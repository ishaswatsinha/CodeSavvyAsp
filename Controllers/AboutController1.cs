using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class AboutController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
