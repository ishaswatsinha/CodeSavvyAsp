using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeSavvyAsp.Controllers
{
    public class SAccountDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public SAccountDetailsController(AppDbContext context)
        {
            _context = context;
        }

        private string GetLoggedInEmail()
        {
            return HttpContext.Session.GetString("StudentEmail");
        }

        public IActionResult Index()
        {
            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "SLogin");

            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Login", "SLogin");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "SLogin");

            var student = _context.Students
                                  .AsNoTracking()
                                  .FirstOrDefault(s => s.Id == id && s.Email == email);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student updatedStudent)
        {
            var sessionEmail = GetLoggedInEmail();
            if (string.IsNullOrEmpty(sessionEmail))
                return RedirectToAction("Login", "SLogin");

            // Validate Model (except Password now)
            ModelState.Remove("Password"); // 👈 Skip password validation
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "⚠ Please fix the validation errors.";
                return View(updatedStudent);
            }

            var student = _context.Students.FirstOrDefault(s => s.Id == updatedStudent.Id && s.Email == sessionEmail);
            if (student == null)
            {
                TempData["ErrorMessage"] = "❌ Student not found.";
                return RedirectToAction("Index");
            }

            student.FirstName = updatedStudent.FirstName;
            student.LastName = updatedStudent.LastName;
            student.Email = updatedStudent.Email;
            student.PhoneNumber = updatedStudent.PhoneNumber;
            student.CountryCode = updatedStudent.CountryCode;

            // 👇 Only update password if user entered a new one
            if (!string.IsNullOrEmpty(updatedStudent.Password))
            {
                student.Password = updatedStudent.Password;
            }

            _context.Students.Update(student);
            _context.SaveChanges();

            if (sessionEmail != updatedStudent.Email)
                HttpContext.Session.SetString("StudentEmail", updatedStudent.Email);

            TempData["SuccessMessage"] = "✅ Profile updated successfully!";
            return RedirectToAction("Index");
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
