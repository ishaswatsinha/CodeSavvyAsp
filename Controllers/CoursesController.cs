using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Course()
        {
            return View();
        }

        public IActionResult CourseDetails()
        {
            return View();
        }

    }
}
