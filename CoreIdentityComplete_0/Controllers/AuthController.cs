using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityComplete_0.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AuthController : Controller
    {
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
