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

        // ✅ Updated EnrolledCourses Method
        public async Task<IActionResult> EnrolledCourses()
        {
            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "SLogin");

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Login", "SLogin");
            }

            var enrolledCourses = await _context.EnrolledCourses
                                                .Where(e => e.StudentId == student.Id)
                                                .Include(e => e.Course)
                                                .ToListAsync(); // ✅ Async version for better performance

            return View(enrolledCourses);
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
