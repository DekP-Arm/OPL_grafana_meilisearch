using Newtonsoft.Json;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;

namespace OPL_grafana_meilisearch.src.Core.Service
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly HttpClient client = new HttpClient();


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUserAsync()
        {
            try
            {

                using HttpResponseMessage response = await client.GetAsync("https://668abaee2c68eaf3211da858.mockapi.io/api/getdata/users");
                var responseBody = await response.Content.ReadAsStringAsync();
                // Console.WriteLine(responseBody);
                var result = JsonConvert.DeserializeObject<List<UserDto>>(responseBody);
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

