using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;

namespace OPL_grafana_meilisearch.src.Core.Service
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUserAsync()
        {
            try
            {
                var userData = await _userRepository.GetAllUserAsync();
                var result = userData.Select(x => new UserDto
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
        public async Task<List<UserDto>> AddUserAsync(string username, string password)
        {
            try
            {
                // Simulate async operation
                await Task.Delay(100);

                var result = new List<UserDto>
            {
                new UserDto
                {
                    Username = username,
                    Password = password
                }
            };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

