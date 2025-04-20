using CodeSavvyAsp.Data;
using Microsoft.AspNetCore.Mvc;
using CodeSavvyAsp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeSavvyAsp.Controllers
{
    public class StudentProgressController : Controller
    {
        private readonly AppDbContext _context;

        public StudentProgressController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Show Progress Dashboard
        public IActionResult Index()
        {
            var email = GetLoggedInEmail();
            var student = _context.Students.FirstOrDefault(s => s.Email == email);

            if (student == null)
                return RedirectToAction("Login", "SLogin");

            var progressList = _context.StudentProgress
                .Where(sp => sp.StudentId == student.Id)
                .Include(sp => sp.Course)
                .ToList();

            return View(progressList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsComplete(int courseId)
        {
            Console.WriteLine($"DEBUG: Marking Course Complete - CourseID Received: {courseId}");

            if (courseId == 0)
            {
                Console.WriteLine("DEBUG: ERROR - Course ID is NULL or Zero!");
                TempData["ErrorMessage"] = "❌ Invalid course ID!";
                return RedirectToAction("Index");
            }

            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("DEBUG: ERROR - Session Email Missing! Redirecting to Login.");
                TempData["ErrorMessage"] = "❌ Please login to continue.";
                return RedirectToAction("Login", "SLogin");
            }

            var student = _context.Students.FirstOrDefault(s => s.Email == email);

            if (student == null)
            {
                Console.WriteLine("DEBUG: Student Not Found!");
                TempData["ErrorMessage"] = "❌ User not found!";
                return RedirectToAction("Index");
            }

            var progress = _context.StudentProgress.FirstOrDefault(sp => sp.StudentId == student.Id && sp.CourseId == courseId);

            if (progress == null)
            {
                Console.WriteLine("DEBUG: Creating New Progress Entry...");
                var newProgress = new StudentProgress
                {
                    StudentId = student.Id,
                    CourseId = courseId,
                    IsCompleted = true,
                    CompletedOn = DateTime.Now
                };

                _context.StudentProgress.Add(newProgress);
            }
            else
            {
                Console.WriteLine("DEBUG: Updating Existing Progress Entry...");
                progress.IsCompleted = true;
                progress.CompletedOn = DateTime.Now;
                _context.StudentProgress.Update(progress);
            }

            _context.SaveChanges();
            Console.WriteLine("DEBUG: Save Changes Done!");

            TempData["SuccessMessage"] = "✅ Course marked as completed!";
            return RedirectToAction("Index");  // ✅ Redirect to Progress Dashboard
        }








        // ✅ Retrieve Student Progress List
        public IActionResult MyProgress()
        {
            var email = GetLoggedInEmail();
            var student = _context.Students.FirstOrDefault(s => s.Email == email);

            if (student == null)
                return RedirectToAction("Login", "SLogin");

            var progressList = _context.StudentProgress
                .Where(sp => sp.StudentId == student.Id && sp.IsCompleted)
                .Include(sp => sp.Course)
                .ToList();

            return View(progressList);
        }

        // ✅ Utility Method: Get Logged-in Student Email
        private string GetLoggedInEmail()
        {
            return User.Identity.Name; // Adjust based on authentication system
        }
    }
}
