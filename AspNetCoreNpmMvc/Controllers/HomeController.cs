using Microsoft.AspNetCore.Mvc;

namespace NpmAspNetMvcTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}