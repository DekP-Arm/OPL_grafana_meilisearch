using Microsoft.AspNetCore.Mvc;
using testAPI.DTOs;
using testAPI.src.Core.Interface;


namespace permissionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FailedController : Controller
    {
        private readonly IFailedService _failedService;
        private readonly ILogger<FailedDto> _logger;

        public FailedController(IFailedService failedService, ILogger<FailedDto> logger)
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
                _logger.LogError(ex, "Error getting All Faileds");
                return BadRequest(err);
            }
        }

    }
}
