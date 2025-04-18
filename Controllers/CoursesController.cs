using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using System.Linq;

namespace CodeSavvyAsp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Course()
        {
            var courses = _context.Courses.ToList(); // ✅ Database se courses fetch karo
            return View(courses);
        }

        public IActionResult CourseDetails(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found!";
                return RedirectToAction("Index","SAcountDetails");
            }

            return View(course); // ✅ Direct course details page pe bhej raha hai, login ka check hata diya!
        }

    }
}
