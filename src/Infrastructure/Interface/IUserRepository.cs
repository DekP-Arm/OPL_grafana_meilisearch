using testAPI.src.Entities;

namespace testAPI.src.Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<List<UserDbo>> GetAllUserAsync();
    }
}
