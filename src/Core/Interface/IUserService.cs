using OPL_grafana_meilisearch.DTOs;

namespace OPL_grafana_meilisearch.src.Core.Interface
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUserAsync();
    }
}
