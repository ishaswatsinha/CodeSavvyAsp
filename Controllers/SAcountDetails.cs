using CodeSavvyAsp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
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
            var email = HttpContext.Session.GetString("StudentEmail");
            if (!string.IsNullOrEmpty(email))
            {
                var student = _context.Instructors.FirstOrDefault(i => i.Email == email);
                return View(student);
            }
            return RedirectToAction("Login", "Slogin"); // If no email, redirect to login

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
            return RedirectToAction("Login", "SLogin");
        }
    }
}
