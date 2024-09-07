using Microsoft.AspNetCore.Mvc;

namespace WEB_253502_YASTREMSKIY.Controllers
{
    public class Product : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
