using CodeSavvyAsp.net.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.net.Controllers
{
    public class SAcountDetails : Controller
    {
        private readonly AppDbContext _context;
        public SAcountDetails(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("InstructorEmail");
            if (!string.IsNullOrEmpty(email))
            {
                var instructor = _context.Instructors.FirstOrDefault(i=>i.Email == email);
                return View(instructor);
            }
            return RedirectToAction("ILogin", "InstructorLogin"); // If no email, redirect to login

        }
        public IActionResult Settings()
        {
            return View();
        }


        public IActionResult EnrolledCourses()
        {
            return View();
        }


        public IActionResult Wishlisht()
        {
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Ilogin","InstructorLogin");
        }
    }
}
