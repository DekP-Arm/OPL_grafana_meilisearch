using Microsoft.AspNetCore.Mvc;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using ILogger = Serilog.ILogger;


namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;

        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var response = new BaseHttpResponse<List<UserDto>>();
            try
            {
                var data = await _userService.GetAllUserAsync();
                response.SetSuccess(data, "Success", "200");
                return Ok(response);
            }
            catch (Exception ex)
            {
                ErrorData err = new ErrorData();
                err.Code = "1-GetAllUsers";
                err.Message = "Error getting Users";
                _logger.Error(ex, "Error getting All Users");
                return BadRequest(err);
            }
        }

    }
}
