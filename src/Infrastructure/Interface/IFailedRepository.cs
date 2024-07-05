using testAPI.src.Entities;

namespace testAPI.src.Infrastructure.Interface
{
    public interface IFailedRepository
    {
        Task<List<FailedDbo>> GetAllFailedAsync();
    }
}
