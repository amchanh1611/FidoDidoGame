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
        PaggingResponse<UserRankDetailIn> HistoryOf(long userId, HistoryOfRequest request);
        UserRankReponse UserRank(long userId, GetUserRankRequest request);
        void CreateEvent(CreateEventRequest request);
        void UpdateEvent(int eventId, UpdateEventRequest request);
        void CreatePointOfRound(CreatePointOfRoundRequest request);
        List<Event> GetEvent();
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

        public void CreatePointOfRound(CreatePointOfRoundRequest request)
        {
            PointOfRound pointOfRound = repository.PointOfRound.Create(mapper.Map<CreatePointOfRoundRequest, PointOfRound>(request));
            repository.Save();

            UserRankOfRoundIn userRankOfRoundIn = new((long)(Helper.DateStatic - pointOfRound.Date).TotalMilliseconds,
                request.UserName!, pointOfRound.Point, pointOfRound.UserId);

            redis.ZSSet($"{KeysRankOfDay}:Round:{request.EventId}", pointOfRound.Point, userRankOfRoundIn);
        }

        public void UpdateRank(UpdateRank request)
        {
            PointOfRound pointOfRound = repository.PointOfRound.FindByCondition(x => x.UserId == request.UserId && x.EventId == request.EventId).FirstOrDefault()!;

            if (pointOfRound is null)
                CreatePointOfRound(mapper.Map<UpdateRank, CreatePointOfRoundRequest>(request));
            else
                UpdatePointOfRound(pointOfRound.Id, mapper.Map<UpdateRank, UpdatePointOfRoundRequest>(request));
        }

        private void UpdatePointOfRound(int pointOfRoundId, UpdatePointOfRoundRequest request)
        {
            PointOfRound pointOfRound = repository.PointOfRound.FindByCondition(x => x.Id == pointOfRoundId).FirstOrDefault()!;

            //Tạo key cho 1 Round
            string key = $"{KeysRankOfDay}:Round:{request.EventId}";

            //Remove old member in sortset in Redis if PointOfDay already exist
            UserRankOfRoundIn userRankOfRoundIn = new((long)(Helper.DateStatic - pointOfRound.Date).TotalMilliseconds,
                request.UserName!, pointOfRound.Point, pointOfRound.UserId);

            redis.ZSDelete(key, userRankOfRoundIn);

            //Update point in MySql 
            pointOfRound.Point += request.Point;
            pointOfRound.Date = request.Date;
            pointOfRound = repository.PointOfRound.Update(pointOfRound);

            repository.Save();

            //Update new member in sortset in Redis
            userRankOfRoundIn = new((long)(Helper.DateStatic - pointOfRound.Date).TotalMilliseconds,
                request.UserName!, pointOfRound.Point, pointOfRound.UserId);

            redis.ZSIncre(key, pointOfRound.Point, userRankOfRoundIn);
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
            List<UserRankOfRoundIn> values = redis.ZSGet<UserRankOfRoundIn>($"{KeysRankOfDay}:Round:{request.EventId}");

            return mapper.Map<List<UserRankOfRoundIn>, List<RankingResponse>>(values).AsQueryable().ApplyPagging(request.Current, request.PageSize);
        }

        public PaggingResponse<UserRankDetailIn> HistoryOf(long userId, HistoryOfRequest request)
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
        public UserRankReponse UserRank(long userId, GetUserRankRequest request)
        {
            PointOfRound pointOfDay = repository.PointOfRound.FindByCondition(x => x.UserId == userId && x.EventId == request.EventId).Include(x => x.User).FirstOrDefault()!;

            UserRankOfRoundIn userRankOfRoundIn = new((long)Helper.DateStatic.Subtract(pointOfDay.Date).TotalMilliseconds, pointOfDay.User!.Name, pointOfDay.Point, pointOfDay.UserId);

            long? rankOfUser = redis.ZsGetRank($"{KeysRankOfDay}:Round:{request.EventId}", userRankOfRoundIn) + 1;
            return new UserRankReponse(rankOfUser, mapper.Map<UserRankOfRoundIn, RankingResponse>(userRankOfRoundIn));
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

        public List<Event> GetEvent()
        {
            return repository.Event.FindAll().ToList();
        }
    }
}
