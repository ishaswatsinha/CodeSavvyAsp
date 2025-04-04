using CodeSavvyAsp.net.Data;
using CodeSavvyAsp.net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace CodeSavvyAsp.net.Controllers
{
    public class InstructorLogin : Controller
    {
        private readonly AppDbContext _context;

        public InstructorLogin(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ILogin()
        {
            return View(); // Login page show karega
        }


        [HttpPost]
        public IActionResult ILogin(Instructor instructor )
        {
            var existingInstructor = _context.Instructors.FirstOrDefault(i => i.Email == instructor.Email
            && i.Password == instructor.Password);

            if (existingInstructor != null)
            {
                TempData["Success"] = "Login success";
                return RedirectToAction("InsIndex", "Instructor");
            }
            ViewBag.ErrorMessage = "Invalid id and Password";
            return View();
        }



        [HttpGet]
        public IActionResult ISignup()
        {
            return View();
        }
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
