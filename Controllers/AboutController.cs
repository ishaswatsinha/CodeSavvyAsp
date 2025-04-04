using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
