using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.net.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
