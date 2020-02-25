using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IS401_TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("PerformAdminAction")] // Method + Route
        [Authorize]
        public async Task<IActionResult> PerformAdminActionAsync()
        {
            await Task.Delay(100); // Simulate Delay
            //.ConfigureAwait(false) is not really needed on AspNetCore anymore.
            // possibly keeping doing in NetCore libraries as the consumers
            // of those libraries could be NetCore WinForms/WPF etc.

            return Ok("Action performed successfully.");
        }
    }
}