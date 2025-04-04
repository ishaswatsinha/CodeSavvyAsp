using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.net.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult InsIndex()
        {
            return View();
        }
        public IActionResult CourseInformation()
        {
            return View();
        }
        public IActionResult CourseBuilder()
        {
            return View();
        }
    }
}
