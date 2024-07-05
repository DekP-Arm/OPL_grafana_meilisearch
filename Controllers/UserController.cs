using Microsoft.AspNetCore.Mvc;
using testAPI.DTOs;
using testAPI.src.Core.Interface;


namespace permissionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserDto> _logger;

        public UserController(IUserService userService, ILogger<UserDto> logger)
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
                _logger.LogError(ex, "Error getting All Users");
                return BadRequest(err);
            }
        }

    }
}
