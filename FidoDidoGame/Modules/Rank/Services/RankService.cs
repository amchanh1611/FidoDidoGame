using AutoMapper;
using FidoDidoGame.Common.Pagination;
using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Modules.Rank.Response;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Ranks.Request;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;

namespace FidoDidoGame.Modules.Ranks.Services
{
    public interface IRankService
    {
        void CreatePointDetail(CreatePointDetailRequest request);
        void UpdateRank(UpdateRank request);
        PaggingResponse<RankingResponse> Ranking(GetRankRequest request);
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
            repository.PointOfDay.Save();

            UserRankOfDayIn userRankOfDayIn = new((long)(new DateTime(2100, 12, 31, 23, 59, 59) - pointOfDay.Date).TotalMilliseconds, request.UserName!, pointOfDay.Point, pointOfDay.Date);
            redis.ZSSet(KeysRankOfDay, pointOfDay.Point, userRankOfDayIn);
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

            //Remove old member in sortset in Redis if PointOfDay already exist
            UserRankOfDayIn userRankOfDayIn = new((long)(new DateTime(2100, 12, 31, 23, 59, 59) - pointOfDay.Date).TotalMilliseconds, request.UserName!, pointOfDay.Point, pointOfDay.Date);

            Console.WriteLine(userRankOfDayIn.Date);

            redis.ZSDelete(KeysRankOfDay, userRankOfDayIn);

            //Update point in MySql 
            pointOfDay.Point += request.Point;
            pointOfDay.Date = request.Date;
            pointOfDay = repository.PointOfDay.Update(pointOfDay);

            repository.User.Save();

            //Update new member in sortset in Redis
            userRankOfDayIn = new((long)(new DateTime(2100, 12, 31, 23, 59, 59) - pointOfDay.Date).TotalMilliseconds, request.UserName!, pointOfDay.Point, pointOfDay.Date);

            Console.WriteLine(userRankOfDayIn.Date);

            redis.ZSIncre(KeysRankOfDay, pointOfDay.Point, userRankOfDayIn);
        }

        public void CreatePointDetail(CreatePointDetailRequest request)
        {
            PointDetail pointDetail = repository.PointDetail.Create(mapper.Map<CreatePointDetailRequest, PointDetail>(request));
            repository.PointDetail.Save();

            UserRankDetailIn userRankDetailIn = new(pointDetail.Date, request.UserName!, pointDetail.Point, pointDetail.IsX2);

            redis.ZSSet($"{KeyRankDetail}:User:{pointDetail.UserId}", (int)request.Date.Subtract(DateTime.UtcNow).TotalSeconds, userRankDetailIn);
        }

        public PaggingResponse<RankingResponse> Ranking(GetRankRequest request)
        {
            List<UserRankOfDayIn> values = redis.ZSGet<UserRankOfDayIn>(KeysRankOfDay);
            return mapper.Map<List<UserRankOfDayIn>, List<RankingResponse>>(values)
                .Where(x => x.Date >= request.DateStart && x.Date <= request.DateEnd).AsQueryable()
                .ApplyPagging(request.Current, request.PageSize); 
        }
    }
}
