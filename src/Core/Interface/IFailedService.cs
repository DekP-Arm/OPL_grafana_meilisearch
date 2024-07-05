using testAPI.DTOs;

namespace testAPI.src.Core.Interface
{
    public interface IFailedService
    {
        Task<List<FailedDto>> GetAllFailedAsync();
    }
}
