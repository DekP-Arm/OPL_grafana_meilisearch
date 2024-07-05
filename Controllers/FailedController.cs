using Microsoft.AspNetCore.Mvc;
using testAPI.DTOs;
using testAPI.src.Core.Interface;
using ILogger = Serilog.ILogger;

namespace permissionAPI.Controllers
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
                err.Code = "1-GetAllFaileds";
                err.Message = "Error getting Faileds";
                _logger.Error(ex, "Error getting All Faileds");
                return BadRequest(err);
            }
        }

    }
}
