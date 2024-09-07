using Microsoft.AspNetCore.Mvc;

namespace WEB_253502_YASTREMSKIY.Controllers
{
    public class Account : Controller
    {
        public IActionResult Logout()
        {
            return NoContent();
        }
    }
}
