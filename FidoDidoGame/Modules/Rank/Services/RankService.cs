using AutoMapper;
using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Modules.Rank.Response;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Ranks.Request;
using FidoDidoGame.Persistents.Redis.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace FidoDidoGame.Modules.Ranks.Services
{
    public interface IRankService
    {
        void CreatePointDetail(CreatePointDetailRequest request);
        void UpdateRank(UpdateRank request);
        void Ranking(/*GetRankRequest request*/);
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

            UserRankOfDayIn userRankOfDayIn = new(pointOfDay.Date, request.UserName!);
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
            UserRankOfDayIn userRankOfDayIn = new(pointOfDay.Date, request.UserName!);
            redis.ZSDelete(KeysRankOfDay, userRankOfDayIn);

            //Update point in MySql 
            pointOfDay.Point += request.Point;
            pointOfDay.Date = request.Date;
            pointOfDay = repository.PointOfDay.Update(pointOfDay);

            repository.User.Save();

            //Update new member in sortset in Redis
            userRankOfDayIn = new(pointOfDay.Date, request.UserName!);
            redis.ZSIncre(KeysRankOfDay, pointOfDay.Point, userRankOfDayIn);
        }

        public void CreatePointDetail(CreatePointDetailRequest request)
        {
            PointDetail pointDetail = repository.PointDetail.Create(mapper.Map<CreatePointDetailRequest, PointDetail>(request));
            repository.PointDetail.Save();

            UserRankDetailIn userRankDetailIn = new(pointDetail.Date, request.UserName!, pointDetail.Point, pointDetail.IsX2);

            redis.ZSSet($"{KeyRankDetail}:User:{pointDetail.UserId}", pointDetail.Id, userRankDetailIn);
        }

        public void Ranking(/*GetRankRequest request*/)
        {
            SortedSetEntry[] a = redis.ZSGet(KeysRankOfDay);
            List<RankingResponse> result =  a.Select(x => new RankingResponse
            { 
                Point = (int)x.Score, 
                UserName = JsonSerializer.Deserialize<UserRankOfDayIn>(x.Element!)!.UserName,
                Date = JsonSerializer.Deserialize<UserRankOfDayIn>(x.Element!)!.Date
            }).OrderBy(x=>x.Date).ToList();
        }
    }
}
