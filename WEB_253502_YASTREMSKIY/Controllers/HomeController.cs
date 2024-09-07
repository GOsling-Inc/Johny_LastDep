using Microsoft.AspNetCore.Mvc;
using WEB_253502_YASTREMSKIY.Helpers;
using WEB_253502_YASTREMSKIY.Models;

namespace WEB_253502_YASTREMSKIY.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Work";
            ViewData["Message"] = "Лабораторная работа №2";
            var listDemo = new List<ListDemo>
            {
                new ListDemo{Id = 1, Name = "Item 1"},
                new ListDemo{Id = 2, Name = "Item 2"},
                new ListDemo{Id = 3, Name = "Item 3"},
            };
            var modelView = new IndexViewModel
            {
                Items = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(listDemo, "Id", "Name")
            };
            Console.WriteLine(ViewBag.Title);
            return View(modelView);
        }
    }
}
