using Microsoft.AspNetCore.Mvc;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using Microsoft.Extensions.Logging;
using Meilisearch;

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FailedController : Controller
    {
        private readonly IFailedService _failedService;
        private readonly ILogger<FailedController> _logger;

        private readonly VaultService _vaultService;
        private readonly MeilisearchClient _meilisearch;

        private static readonly ActivitySource activitySource = new ActivitySource("APITracing");

        // Extracts context in HTTP headers and injects context into HTTP headers.
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

        private readonly string meilisearchHost ="";

        private readonly string meilisearchApiKey= "";

        public FailedController(IFailedService failedService, ILogger<FailedController> logger, VaultService vaultService)
        {
            _failedService = failedService;
            _logger = logger;
            _vaultService = vaultService;

            // meilisearchHost = configuration.GetValue<string>("MeilisearchClient:Host");
            // meilisearchApiKey = configuration.GetValue<string>("MeilisearchClient:ApiKey");

            var secret = _vaultService.GetSecretAsync("meilisearch-client").Result;
            meilisearchHost = secret.Data.Data["Host"].ToString();
            meilisearchApiKey = secret.Data.Data["ApiKey"].ToString();
            _meilisearch = new MeilisearchClient(meilisearchHost, meilisearchApiKey);
        }

        [HttpGet("GetAllFailed")]
        public async Task<IActionResult> GetAllFailedAsync()
        {
            var response = new BaseHttpResponse<List<FailedDto>>();
            try
            {
                using (var activity = activitySource.StartActivity("FailedController-GetAllFailedAsync",ActivityKind.Server))
                {
                    activity?.SetTag("component", "FailedController");
                    activity?.SetTag("http.method", "GET");
                    activity?.SetTag("http.url", "/api/failed/GetAllFailed");
                    
                    using (var failServiceActivity = activitySource.StartActivity("Call FailService", ActivityKind.Internal))
                    {
                        // Tag the activity with the name of the service and the method being called
                        failServiceActivity?.SetTag("internal.service.name", "FailedService");
                        failServiceActivity?.SetTag("internal.service.method", "GetAllUserAsync");

                        // Call the service
                        var data = await _failedService.GetAllFailedAsync();
                        response.Data = data;
                    }
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                using (var activity = activitySource.StartActivity("FailedController-GetAllFailedAsync-Error",ActivityKind.Server))
                {
                    activity.SetTag("error", true);
                    activity.SetTag("error.message", ex.Message);
                    activity.SetTag("error.stacktrace", ex.StackTrace);
                    var err = new ErrorData
                    {
                        Code = "1-GetAllUsers",
                        Message = "Error getting Users"
                    };
                    _logger.LogError(ex, "Error getting users");

                    using (var meilisearchActivity = activitySource.StartActivity("Send Data to Meilisearch", ActivityKind.Client))
                        {
                            DateTime currentDateTime = DateTime.Now;
                            var formattedDateTime = currentDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                            var error_log = new[]
                            {
                                new ErrorLogMeilisearchDto
                                {
                                    CodeId = Guid.NewGuid().ToString("N"),
                                    Api = "1-GetAllFaileds",
                                    Message = "Error getting All Faileds",
                                    dateTime = formattedDateTime,
                                }
                            };
                            var index = _meilisearch.Index("Error_log");
                            await index.AddDocumentsAsync(error_log);
                        }
                    return StatusCode(StatusCodes.Status500InternalServerError, err);
                }
            }
        }
        [HttpPost("AddFailed")]
        public async Task<IActionResult> AddFailedAsync(string username, string password)
        {
            var response = new BaseHttpResponse<List<FailedDto>>();
            try
            {
                using (var activity = activitySource.StartActivity("FailedController-AddFailedAsync",ActivityKind.Server))
                {
                    activity?.SetTag("component", "FailedController");
                    activity?.SetTag("http.method", "POST");
                    activity?.SetTag("http.url", "/api/failed/AddFailedAsync");


                    using (var userServiceActivity = activitySource.StartActivity("Call FailedService", ActivityKind.Internal))
                    {
                        // Tag the activity with the name of the service and the method being called
                        userServiceActivity?.SetTag("internal.service.name", "FailedService");
                        userServiceActivity?.SetTag("internal.service.method", "AddFailedAsync");

                        var data = await _failedService.AddFailedAsync(username,password);
                    }
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                using (var activity = activitySource.StartActivity("FailedController-AddUser-Error",ActivityKind.Server))
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
