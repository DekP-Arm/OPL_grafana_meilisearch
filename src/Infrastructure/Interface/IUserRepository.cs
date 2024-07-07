using OPL_grafana_meilisearch.src.Entities;

namespace OPL_grafana_meilisearch.src.Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<List<UserDbo>> GetAllUserAsync();
        
    }
}
