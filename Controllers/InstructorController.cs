using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeSavvyAsp.Controllers
{
    public class InstructorController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public InstructorController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Instructor Dashboard (My Profile)
        public IActionResult Index()
        {
            // Fetch the instructor's email from the session
            string email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            // Find the instructor by their email
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.Email == email);

            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            return View(instructor);
        }

        // ✅ My Courses: Show a list of courses taught by the logged-in instructor
        public IActionResult MyCourses()
        {
            var email = HttpContext.Session.GetString("InstructorEmail");
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.Email == email);

            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            // Retrieve the courses associated with this instructor
            var courses = _context.InstructorCourses
                .Where(c => c.InstructorId == instructor.Id)
                .ToList();

            return View(courses);
        }

        // GET: Add New Course
        
        public IActionResult AddCourse()
        {
            return View();
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourse(InstructorCourse course)
        {
            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            // No InstructorId assignment required since it's removed from the model
            // course.InstructorId = instructor.Id;  <-- Remove this line

            if (ModelState.IsValid)
            {
                _context.InstructorCourses.Add(course);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Course added successfully!";
                return RedirectToAction("MyCourses");
            }

            // Debugging validation errors
            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                var errors = entry.Value.Errors;

                foreach (var error in errors)
                {
                    Console.WriteLine($"[Model Error] Field: {key} - Error: {error.ErrorMessage}");
                }
            }

            TempData["ErrorMessage"] = "Please fix the errors in the form.";
            return View(course);
        }























        // ✅ Edit Instructor Profile (from session)
        [HttpGet]
        public IActionResult Edit()
        {
            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors
                .FirstOrDefault(i => i.Email == email);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // ✅ Edit Instructor Profile by ID (admin use or deep link)
        [HttpGet]
        public IActionResult EditById(int id)
        {
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("InstructorEmail", instructor.Email);
            return View("Edit", instructor);
        }

        // ✅ Save updated instructor info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Instructor updatedInstructor)
        {
            if (ModelState.IsValid)
            {
                var instructor = _context.Instructors
                    .FirstOrDefault(i => i.Id == updatedInstructor.Id);

                if (instructor == null)
                {
                    return NotFound();
                }

                instructor.FirstName = updatedInstructor.FirstName;
                instructor.LastName = updatedInstructor.LastName;
                instructor.Email = updatedInstructor.Email;
                instructor.PhoneNumber = updatedInstructor.PhoneNumber;
                instructor.CountryCode = updatedInstructor.CountryCode;
                instructor.Password = updatedInstructor.Password;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Index");
            }

            return View(updatedInstructor);
        }
    }
}
