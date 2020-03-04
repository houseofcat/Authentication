using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}