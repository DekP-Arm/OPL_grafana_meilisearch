
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OPL_grafana_meilisearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretController : ControllerBase
    {
        private readonly VaultService _vaultService;

        public SecretController(VaultService vaultService)
        {
            _vaultService = vaultService;
        }

        [HttpGet("{path}")]
        public async Task<IActionResult> GetSecret(string path)
        {
            var secret = await _vaultService.GetSecretAsync(path);
            return Ok(secret.Data.Data);
        }
    }
}