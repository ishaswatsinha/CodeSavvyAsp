using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CodeSavvyAsp.Controllers
{
    public class InstructorLogin : Controller
    {
        private readonly AppDbContext _context;

        public InstructorLogin(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login Page
        [HttpGet]
        public IActionResult ILogin()
        {
            return View();
        }

        // POST: Handle Login
        [HttpPost]
        public IActionResult ILogin(Instructor instructor)
        {
            var existingInstructor = _context.Instructors.FirstOrDefault(i =>
                i.Email == instructor.Email && i.Password == instructor.Password);

            if (existingInstructor != null)
            {
                // Store email in session
                HttpContext.Session.SetString("InstructorEmail", existingInstructor.Email);

                TempData["Success"] = "Login successful!";
                return RedirectToAction("Index", "Instructor");
            }

            ViewBag.ErrorMessage = "Invalid Email or Password";
            return View();
        }

        // GET: Signup Page
        [HttpGet]
        public IActionResult ISignup()
        {
            return View();
        }

        // POST: Handle Signup
        [HttpPost]
        public IActionResult ISignup(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                _context.SaveChanges();

                TempData["Success"] = "Instructor registered successfully!";
                return RedirectToAction("ILogin");
            }

            return View();
        }

        public IActionResult VerifyPhone()
        {
            return View();
        }

        public IActionResult ForgotPassIns()
        {
            return View();
        }

       
    }
}
