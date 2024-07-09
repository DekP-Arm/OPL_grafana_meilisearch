using Newtonsoft.Json;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using OPL_grafana_meilisearch.src.Infrastructure.Interface;
using System.Diagnostics;
using OpenTelemetry.Context.Propagation;
using System.Net.Http;
using OpenTelemetry;



namespace OPL_grafana_meilisearch.src.Core.Service
{
    
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly HttpClient client = new HttpClient();

        private static readonly ActivitySource activitySource = new ActivitySource("APITracing");
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUserAsync()
        {
            using (var activity = activitySource.StartActivity("GetAllUserAsync", ActivityKind.Internal))
            {
                activity?.SetTag("component", "UserService");
                activity?.SetTag("http.method", "GET");
                activity?.SetTag("http.url", "https://66861e8b83c983911b00e003.mockapi.io/api/name/username");

                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://66861e8b83c983911b00e003.mockapi.io/api/name/username");
                    // ส่ง context ของการติดตามไปพร้อมกับคำขอ HTTP
                    Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), request.Headers, (headers, key, value) => headers.Add(key, value));

                    using HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();

                    activity?.SetTag("http.status_code", (int)response.StatusCode);

                    var result = JsonConvert.DeserializeObject<List<UserDto>>(responseBody);
                    return result;
                }
                catch (Exception ex)
                {
                    activity?.SetTag("error", true);
                    activity?.SetTag("error.message", ex.Message);
                    activity?.SetTag("error.stacktrace", ex.StackTrace);
                    throw;
                }
            }
        }
        public async Task<List<UserDto>> AddUserAsync(string username, string password)
        {
            using (var activity = activitySource.StartActivity("AddUserAsync", ActivityKind.Internal))
            {
                activity?.SetTag("component", "UserService");
                activity?.SetTag("http.method", "POST");
                activity?.SetTag("http.url", "null");
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
                    activity?.SetTag("error", true);
                    activity?.SetTag("error.message", ex.Message);
                    activity?.SetTag("error.stacktrace", ex.StackTrace);
                    throw ex;
                }
            }


            
        }
    }
}

