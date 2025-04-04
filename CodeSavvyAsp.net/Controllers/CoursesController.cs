using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.net.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult CourseDetails()
        {
            return View();
        }


    }
}
