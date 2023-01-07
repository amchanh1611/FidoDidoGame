using AutoMapper;
using FidoDidoGame.Common.Extensions;
using FidoDidoGame.Common.Pagination;
using FidoDidoGame.Modules.Rank.Entities;
using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Modules.Rank.Response;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Ranks.Request;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FidoDidoGame.Modules.Ranks.Services
{
    public interface IRankService
    {
        void CreatePointDetail(CreatePointDetailRequest request);
        void UpdateRank(UpdateRank request);
        PaggingResponse<RankingResponse> Ranking(GetRankRequest request);
        PaggingResponse<UserRankDetailIn> HistoryOf(int userId, HistoryOfRequest request);
        UserRankReponse UserRank(int userId, GetUserRankRequest request);
        void CreateEvent(CreateEventRequest request);
        void UpdateEvent(int eventId, UpdateEventRequest request);
    }
    public class RankService : IRankService
    {
        public const string KeysRankOfDay = "Rank";
        public const string KeyRankDetail = "History";

        private readonly IRedisService redis;
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public RankService(IRedisService redis, IRepository repository, IMapper mapper)
        {
            this.redis = redis;
            this.repository = repository;
            this.mapper = mapper;
        }

        private void CreatePointOfDay(CreatePointOfDayRequest request)
        {
            PointOfDay pointOfDay = repository.PointOfDay.Create(mapper.Map<CreatePointOfDayRequest, PointOfDay>(request));
            repository.Save();

            UserRankOfDayIn userRankOfDayIn = new((long)(Helper.DateStatic - pointOfDay.Date).TotalMilliseconds,
                request.UserName!, pointOfDay.Point, pointOfDay.UserId, pointOfDay.Date);

            // Gets event id
            Event eventId = repository.Event
                .FindByCondition(x => x.DateStart <= pointOfDay.Date && x.DateEnd >= pointOfDay.Date).FirstOrDefault()!;

            redis.ZSSet($"{KeysRankOfDay}:Round:{eventId.Id}", pointOfDay.Point, userRankOfDayIn);
        }


        public void UpdateRank(UpdateRank request)
        {
            PointOfDay pointOfDay = repository.PointOfDay.FindByCondition(x => x.UserId == request.UserId && x.Date.Date == request.Date.Date).FirstOrDefault()!;

            if (pointOfDay is null)
                CreatePointOfDay(mapper.Map<UpdateRank, CreatePointOfDayRequest>(request));
            else
                UpdatePointOfDay(pointOfDay.Id, mapper.Map<UpdateRank, UpdatePointOfDayRequest>(request));
        }

        private void UpdatePointOfDay(int pointOfDayId, UpdatePointOfDayRequest request)
        {
            PointOfDay pointOfDay = repository.PointOfDay.FindByCondition(x => x.Id == pointOfDayId).FirstOrDefault()!;

            // Gets event id
            Event eventId = repository.Event
                .FindByCondition(x => x.DateStart <= pointOfDay.Date && x.DateEnd >= pointOfDay.Date).FirstOrDefault()!;

            string key = $"{KeysRankOfDay}:Round:{eventId.Id}";

            //Remove old member in sortset in Redis if PointOfDay already exist
            UserRankOfDayIn userRankOfDayIn = new((long)(Helper.DateStatic - pointOfDay.Date).TotalMilliseconds,
                request.UserName!, pointOfDay.Point, pointOfDay.UserId, pointOfDay.Date);

            redis.ZSDelete(key, userRankOfDayIn);

            //Update point in MySql 
            pointOfDay.Point += request.Point;
            pointOfDay.Date = request.Date;
            pointOfDay = repository.PointOfDay.Update(pointOfDay);

            repository.Save();

            //Update new member in sortset in Redis
            userRankOfDayIn = new((long)(Helper.DateStatic - pointOfDay.Date).TotalMilliseconds, 
                request.UserName!, pointOfDay.Point,pointOfDay.UserId, pointOfDay.Date);

            redis.ZSIncre(key, pointOfDay.Point, userRankOfDayIn);
        }

        public void CreatePointDetail(CreatePointDetailRequest request)
        {
            PointDetail pointDetail = repository.PointDetail.Create(mapper.Map<CreatePointDetailRequest, PointDetail>(request));
            repository.Save();

            UserRankDetailIn userRankDetailIn = new(pointDetail.Date, request.UserName!, pointDetail.Point, pointDetail.IsX2, pointDetail.SpecialStatus);

            // Lưu score là Timestamp phải lấy thời gian cố định (DateStatic) trừ đi, lúc lấy ra hay so sánh cũng phải dùng DateStatic mới chính xác
            redis.ZSSet($"{KeyRankDetail}:User:{pointDetail.UserId}", (long)Helper.DateStatic.Subtract(pointDetail.Date).TotalMilliseconds, userRankDetailIn);
        }

        public PaggingResponse<RankingResponse> Ranking(GetRankRequest request)
        {
            List<UserRankOfDayIn> values = redis.ZSGet<UserRankOfDayIn>(KeysRankOfDay);

            return mapper.Map<List<UserRankOfDayIn>, List<RankingResponse>>(values)
                .Where(x => x.Date >= request.DateStart && x.Date <= request.DateEnd)
                .GroupBy(key => key.UserId)
                .Select(grp => new RankingResponse { UserName = grp.Select(x => x.UserName).FirstOrDefault(), Point = grp.Sum(x => x.Point), Date = grp.Select(x => x.Date).LastOrDefault() })
                .OrderByDescending(x => x.Point).ThenBy(x => x.Date)
                .AsQueryable()
                .ApplyPagging(request.Current, request.PageSize);
        }

        public PaggingResponse<UserRankDetailIn> HistoryOf(int userId, HistoryOfRequest request)
        {
            //Convert DateTime to timestamp 
            /// <Sumary>
            /// Lấy time cố định trừ cho DateTime request 
            /// </Sumary>
            long dateStart = (long)Helper.DateStatic.Subtract(request.DateStart!.Value).TotalMilliseconds;
            long dateEnd = (long)Helper.DateStatic.Subtract(request.DateEnd!.Value).TotalMilliseconds;


            ///<summary>
            /// Convert to timestamp thì DateEnd sẽ bé hơn DateStart nên truyền vào min = dateEnd, max = dateStart
            ///</summary>
            return redis.ZSGetByScores<UserRankDetailIn>($"{KeyRankDetail}:User:{userId}", dateEnd, dateStart).AsQueryable()
                .ApplyPagging(request.Current, request.PageSize);
        }
        public UserRankReponse UserRank(int userId, GetUserRankRequest request)
        {
            PointOfDay pointOfDay = repository.PointOfDay.FindByCondition(x => x.UserId == userId).Include(x => x.User).FirstOrDefault()!;
            List<UserRankOfDayIn> values = redis.ZSGet<UserRankOfDayIn>(KeysRankOfDay);

            List<RankingResponse> rank = mapper.Map<List<UserRankOfDayIn>, List<RankingResponse>>(values)
                .Where(x => x.Date >= request.DateStart && x.Date <= request.DateEnd)
                .GroupBy(key => key.UserName)
                .Select(grp => new RankingResponse { UserName = grp.Key, Point = grp.Sum(x => x.Point), Date = grp.Select(x => x.Date).LastOrDefault() })
                .OrderByDescending(x => x.Point).ThenBy(x => x.Date).ToList();

            RankingResponse rankingOfUser =  rank.Where(x => x.UserName == pointOfDay.User!.Name).FirstOrDefault()!;
            return new UserRankReponse(rank.IndexOf(rankingOfUser) + 1, rankingOfUser)!;
        }
        public void CreateEvent(CreateEventRequest request)
        {
            repository.Event.Create(mapper.Map<CreateEventRequest, Event>(request));
            repository.Save();
        }
        public void UpdateEvent(int eventId, UpdateEventRequest request)
        {
            Event entity = repository.Event.FindByCondition(x => x.Id == eventId).FirstOrDefault()!;

            repository.Event.Update(mapper.Map(request, entity));
            repository.Save();
        }

    }
}
