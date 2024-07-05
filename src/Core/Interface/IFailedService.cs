using OPL_grafana_meilisearch.DTOs;

namespace OPL_grafana_meilisearch.src.Core.Interface
{
    public interface IFailedService
    {
        Task<List<FailedDto>> GetAllFailedAsync();
    }
}
