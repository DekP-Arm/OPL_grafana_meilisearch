using testAPI.DTOs;

namespace testAPI.src.Core.Interface
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUserAsync();
    }
}
