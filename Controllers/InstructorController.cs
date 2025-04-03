using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
