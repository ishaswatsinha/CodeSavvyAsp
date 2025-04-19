using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
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

            ModelState.Remove("Password"); // ✅ Skip password validation
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

        public IActionResult EnrolledCourses()
        {
            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "SLogin");

            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            if (student == null)
                return RedirectToAction("Login", "SLogin");

            // ✅ Fetch enrolled courses from Enrollments table
            var enrolledCourseIds = _context.Enrollments
                                            .Where(e => e.StudentId == student.Id)
                                            .Select(e => e.CourseId)
                                            .ToList();

            var enrolledCourses = _context.InstructorCourses
                                          .Where(c => enrolledCourseIds.Contains(c.Id))
                                          .ToList();

            return View(enrolledCourses);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "SLogin");
        }



        public IActionResult BuyNow()

        {
            var courses = _context.InstructorCourses.ToList();//fetch all course by instructor
            return View(courses);//pass course to view
        }












        [HttpGet]
        public IActionResult GetEnrollPage(int courseId) // ✅ New unique method name
        {
            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "SLogin");

            var student = _context.Students.AsNoTracking().FirstOrDefault(s => s.Email == email);
            if (student == null)
                return RedirectToAction("Login", "SLogin");

            var course = _context.InstructorCourses.AsNoTracking().FirstOrDefault(c => c.Id == courseId);
            if (course == null)
                return RedirectToAction("BuyNow");

            return View(course);  // ✅ Show enrollment confirmation page
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnrollNow(int courseId) // ✅ Kept for enrollment submission
        {
            var email = GetLoggedInEmail();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "SLogin");

            var student = _context.Students.FirstOrDefault(s => s.Email == email);
            if (student == null)
                return RedirectToAction("Login", "SLogin");

            var course = _context.InstructorCourses.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
                return RedirectToAction("BuyNow");

            var alreadyEnrolled = _context.Enrollments.Any(e => e.StudentId == student.Id && e.CourseId == courseId);
            if (alreadyEnrolled)
            {
                TempData["ErrorMessage"] = "❌ You are already enrolled in this course!";
                return RedirectToAction("EnrolledCourses");
            }

            var enrollment = new Enrollments
            {
                StudentId = student.Id,
                CourseId = courseId
            };

            try
            {
                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "✅ Successfully enrolled!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "❌ Enrollment failed! Please try again.";
                Console.WriteLine($"Error while enrolling: {ex.Message}");
            }

            return RedirectToAction("EnrolledCourses");
        }




    }
}