using testAPI.DTOs;
using testAPI.src.Core.Interface;
using testAPI.src.Infrastructure.Interface;

namespace testAPI.src.Core.Service
{
    public class UserService : IUserService
    {
        public readonly  IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUserAsync()
        {
            try{
                var userData = await _userRepository.GetAllUserAsync();
                var result = userData.Select( x => new UserDto
                {
                    UserId = x.UserId,
                    Username = x.Username,
                    Password = x.Password
                }
                ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
