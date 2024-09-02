using Microsoft.AspNetCore.Mvc;

namespace WEB_253502_YASTREMSKIY.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
