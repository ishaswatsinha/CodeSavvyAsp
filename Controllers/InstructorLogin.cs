using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class InstructorLogin : Controller
    {
        private readonly AppDbContext _context;

        public InstructorLogin(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Login Page
        [HttpGet]
        public IActionResult ILogin()
        {
            return View(); //login page show hoga
        }

        // ✅ POST: Handle Login
        [HttpPost]
        public IActionResult ILogin(Instructor instructor)
        {
            var existingInstructor = _context.Instructors.FirstOrDefault(i => i.Email == instructor.Email
                && i.Password == instructor.Password);

            if (existingInstructor != null)
            {
                TempData["Success"] = "Login success";
                return RedirectToAction("Index", "Instructor");
            }

            ViewBag.ErrorMessage = "Invalid ID or Password";
            return View();
        }

        // ✅ GET: Signup Page
        [HttpGet]
        public IActionResult ISignup()
        {
            return View();
        }

        // ✅ POST: Handle Signup
        [HttpPost]
        public IActionResult ISignup(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                _context.SaveChanges();
                TempData["Success"] = "Instructor registered successfully";
                return RedirectToAction("ILogin");
            }

            return View();
        }

        // ✅ Phone verification view
        public IActionResult VerifyPhone()
        {
            return View();
        }

        // ✅ Forgot password view
        public IActionResult ForgotPassIns()
        {
            return View();
        }
    }
}
