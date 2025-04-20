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



        public IActionResult MyCourses()
        {
            // ✅ Check if instructor is logged in
            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            // ✅ Fetch the instructor details based on email
            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            // ✅ Fetch only courses related to this instructor
            var courses = _context.InstructorCourses
                .Where(c => c.InstructorId == instructor.Id)  // Filter courses by InstructorId
                .ToList();



            return View(courses);
        }



        // ✅ GET: Load Add Course Page
        public IActionResult AddCourse()
        {
            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor not found. Please log in again.";
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            // ✅ Pass Instructor Info to View
            ViewData["InstructorId"] = instructor.Id;  // Ensure InstructorId is available
            ViewData["InstructorEmail"] = email;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(InstructorCourse course)
        {
            Console.WriteLine("DEBUG: InstructorId received = " + course.InstructorId);

            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor not found. Please log in again.";
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            Console.WriteLine("DEBUG - Received InstructorId: " + course.InstructorId);

            ModelState.Remove("Instructor"); // ✅ Navigation property validation hatao

            if (ModelState.IsValid)
            {
                course.InstructorId = instructor.Id;

                // ✅ URL Directly Store Karna Hai
                // Bas `course.ImageUrl` ka data store kar do, file handling hatao

                _context.InstructorCourses.Add(course);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Course added successfully!";
                return RedirectToAction("MyCourses");
            }

            TempData["ErrorMessage"] = "Invalid data, please check and try again.";

            ViewData["InstructorId"] = instructor.Id;
            ViewData["InstructorEmail"] = email;

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




        // ✅ GET: Confirm Delete Course
        [HttpGet]
        public IActionResult DeleteCourse(int id)
        {
            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var course = _context.InstructorCourses.FirstOrDefault(c => c.Id == id && c.InstructorId == instructor.Id);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found or access denied.";
                return RedirectToAction("MyCourses");
            }

            return View(course);
        }

        // ✅ POST: Delete Course Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCourseConfirmed(int id)
        {
            var email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var course = _context.InstructorCourses.FirstOrDefault(c => c.Id == id && c.InstructorId == instructor.Id);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found or access denied.";
                return RedirectToAction("MyCourses");
            }

            _context.InstructorCourses.Remove(course);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course deleted successfully!";
            return RedirectToAction("MyCourses");
        }


        public IActionResult VideoDetails(int id)
        {
            var course = _context.InstructorCourses.FirstOrDefault(c => c.Id == id);
            if (course == null || string.IsNullOrEmpty(course.VideoUrl))
            {
                return NotFound("Video not available.");
            }
            return View(course); // ✅ Open Video Details View
        }




        public IActionResult CourseDetails(int id)
        {
            var course = _context.InstructorCourses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                TempData["ErrorMessage"] = "❌ Course not found!";
                return RedirectToAction("BuyNow");
            }

            return View(course);
        }


    }
}
