using Microsoft.AspNetCore.Mvc;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using ILogger = Serilog.ILogger;
using Meilisearch;

namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FailedController : Controller
    {
        private readonly IFailedService _failedService;
        private readonly ILogger _logger;
        private readonly MeilisearchClient _meilisearch;

        public FailedController(IFailedService failedService, ILogger logger,IConfiguration configuration)
        {
            _failedService = failedService;
            _logger = logger;
            var meilisearchHost = configuration.GetValue<string>("MeilisearchClient:Host");
            var meilisearchApiKey = configuration.GetValue<string>("MeilisearchClient:ApiKey");
            _meilisearch = new MeilisearchClient(meilisearchHost, meilisearchApiKey);
        }

        [HttpGet("GetAllFailed")]
        public async Task<IActionResult> GetAllFailedAsync()
        {
            var response = new BaseHttpResponse<List<FailedDto>>();
            try
            {
                var data = await _failedService.GetAllFailedAsync();
                response.SetSuccess(data, "Success", "200");
                var index = _meilisearch.Index("FailedLogs");
                await index.AddDocumentsAsync(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new ErrorData
                {
                    Code = "1-GetAllFaileds",
                    Message = "Error getting All Faileds",
                };
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
                _logger.Error(ex, "Error getting All Users");
                return BadRequest(err);
            }
        }
        [HttpPost("AddFailed")]
        public async Task<IActionResult> AddFailedAsync(string username, string password)
        {
            var response = new BaseHttpResponse<List<FailedDto>>();
            try
            {
   
                var data = await _failedService.GetAllFailedAsync();
                response.SetSuccess(data, "Success", "200");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new ErrorData
                {
                    Code = "1-AddFailed",
                    Message = "Error adding faileds"
                };
                _logger.Error(ex, "Error adding faileds");
                return BadRequest(err);
            }
        }

    }
}
