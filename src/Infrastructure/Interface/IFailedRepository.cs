using OPL_grafana_meilisearch.src.Entities;

namespace OPL_grafana_meilisearch.src.Infrastructure.Interface
{
    public interface IFailedRepository
    {
        Task<List<FailedDbo>> GetAllFailedAsync();
    }
}
