using Microsoft.AspNetCore.Mvc;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using ILogger = Serilog.ILogger;

namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FailedController : Controller
    {
        private readonly IFailedService _failedService;
        private readonly ILogger _logger;

        public FailedController(IFailedService failedService, ILogger logger)
        {
            _failedService = failedService;
            _logger = logger;

        }

        [HttpGet("GetAllFailed")]
        public async Task<IActionResult> GetAllFailedAsync()
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
                ErrorData err = new ErrorData();
                err.Code = "500 : internal server error";
                err.Message = "500 : Error getting Faileds";
                _logger.Error(ex, "Error getting All Faileds");
                return StatusCode(500, err);
            }
        }
        [HttpPost("AddFailed")]
        public async Task<IActionResult> AddFailedAsync(string username, string password)
        {
            var response = new BaseHttpResponse<List<FailedDto>>();
            try
            {

                var data = await _failedService.AddFailedAsync(username, password);
                response.SetSuccess(data, "Success", "200");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new ErrorData
                {
                    Code = "Errors 500 : Internal Server Errors",
                    Message = "Error adding faileds"
                };
                _logger.Error(ex, "500 : Internal Server Errors");
                return StatusCode(500, err);
            }
        }

    }
}
