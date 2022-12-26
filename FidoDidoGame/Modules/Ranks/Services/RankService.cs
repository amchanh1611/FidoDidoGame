using FidoDidoGame.Common.Pagination;
using FidoDidoGame.Modules.Ranks.Request;
using FidoDidoGame.Modules.Users.Entities;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;

namespace FidoDidoGame.Modules.Ranks.Services
{
    public interface IRankService
    {

    }
    public class RankService
    {
        private readonly IRedisService redis;
        private readonly IRepository repository;

        public RankService(IRedisService redis, IRepository repository)
        {
            this.redis = redis;
            this.repository = repository;
        }

        public void Ranking(GetRankRequest request)
        {

        }
    }
}
