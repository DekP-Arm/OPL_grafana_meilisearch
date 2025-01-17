using Microsoft.AspNetCore.Mvc;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using Meilisearch;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;


namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly MeilisearchClient _meilisearch;

        private static readonly ActivitySource activitySource = new ActivitySource("APITracing");

        // Extracts context in HTTP headers and injects context into HTTP headers.
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

        private readonly string meilisearchHost ="";

        private readonly string meilisearchApiKey= "";




    
        
        public UserController(IUserService userService, ILogger<UserController> logger,IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;

            meilisearchHost = configuration.GetValue<string>("MeilisearchClient:Host");
            meilisearchApiKey = configuration.GetValue<string>("MeilisearchClient:ApiKey");
            _meilisearch = new MeilisearchClient(meilisearchHost, meilisearchApiKey);

            
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var response = new BaseHttpResponse<List<UserDto>>();
            try
            {
                using (var activity = activitySource.StartActivity("UserController-GetAllUser",ActivityKind.Server))
                {
                    activity?.SetTag("component", "UserController");
                    activity?.SetTag("http.method", "GET");
                    activity?.SetTag("http.url", "/api/User/GetAllUser");
                    

                    using (var userServiceActivity = activitySource.StartActivity("Call UserService", ActivityKind.Internal))
                    {
                        // Tag the activity with the name of the service and the method being called
                        userServiceActivity?.SetTag("internal.service.name", "UserService");
                        userServiceActivity?.SetTag("internal.service.method", "GetAllUserAsync");

                        // Call the service
                        var data = await _userService.GetAllUserAsync();
                        response.SetSuccess(data, "Success", "200");
                    }
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                using (var activity = activitySource.StartActivity("UserController-GetAllUser-Error",ActivityKind.Server))
                {
                    activity.SetTag("error", true);
                    activity.SetTag("error.message", ex.Message);
                    activity.SetTag("error.stacktrace", ex.StackTrace);
                    var err = new ErrorData
                    {
                        Code = "1-GetAllUsers",
                        Message = "Error getting Users"
                    };
                    _logger.LogError(ex, "Error getting All Users");

                    using (var meilisearchActivity = activitySource.StartActivity("Send Data to Meilisearch", ActivityKind.Client))
                        {
                            var error_log = new[]
                            {
                                new ErrorLogMeilisearchDto
                                {
                                    CodeId = "1-GetAllUsers",
                                    Message = "Error getting Users",
                                }
                            };
                            var index = _meilisearch.Index("Error_log");
                            await index.AddDocumentsAsync(error_log);
                        }
                    return StatusCode(StatusCodes.Status500InternalServerError, err);
                }
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync(string username, string password)
        {
            var response = new BaseHttpResponse<List<UserDto>>();
            try
            {
                using (var activity = activitySource.StartActivity("UserController-AddUser",ActivityKind.Server))
                {
                    activity?.SetTag("component", "UserController");
                    activity?.SetTag("http.method", "POST");
                    activity?.SetTag("http.url", "/api/User/AddUser");


                    using (var userServiceActivity = activitySource.StartActivity("Call UserService", ActivityKind.Internal))
                    {
                        // Tag the activity with the name of the service and the method being called
                        userServiceActivity?.SetTag("internal.service.name", "UserService");
                        userServiceActivity?.SetTag("internal.service.method", "AddUserAsync");

                        var data = await _userService.AddUserAsync(username,password);
                    }
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                using (var activity = activitySource.StartActivity("UserController-AddUser-Error",ActivityKind.Server))
                {
                    activity.SetTag("error", true);
                    activity.SetTag("error.message", ex.Message);
                    activity.SetTag("error.stacktrace", ex.StackTrace);

                    var err = new ErrorData
                    {
                        Code = "1-AddUser",
                        Message = "Error adding Users"
                    };

                    _logger.LogError(ex, "Error adding users");

                    using (var meilisearchActivity = activitySource.StartActivity("Send Data to Meilisearch", ActivityKind.Client))
                        {

                        }

                    
                    return StatusCode(StatusCodes.Status500InternalServerError, err);
                }
            }
        }
    }
}
