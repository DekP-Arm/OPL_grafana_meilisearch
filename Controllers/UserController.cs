using Microsoft.AspNetCore.Mvc;
using OPL_grafana_meilisearch.DTOs;
using OPL_grafana_meilisearch.src.Core.Interface;
using ILogger = Serilog.ILogger;
using Meilisearch;
namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly MeilisearchClient _meilisearch;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
            _meilisearch = new MeilisearchClient("http://localhost:7700", "masterKey");
            
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var response = new BaseHttpResponse<List<UserDto>>();
            try
            {
                var data = await _userService.GetAllUserAsync();
                response.SetSuccess(data, "Success", "200");
                var index = _meilisearch.Index("Users");
                await index.AddDocumentsAsync(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new ErrorData
                {
                    Code = "1-GetAllUsers",
                    Message = "Error getting Users"
                };
                _logger.Error(ex, "Error getting All Users");
                return BadRequest(err);
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync(string username, string password)
        {
            var response = new BaseHttpResponse<List<UserDto>>();
            try
            {
   
                var data = await _userService.AddUserAsync(username,password);
                response.SetSuccess(data, "Success", "200");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new ErrorData
                {
                    Code = "1-AddUser",
                    Message = "Error adding users"
                };
                _logger.Error(ex, "Error adding users");
                return BadRequest(err);
            }
        }
    }
}
