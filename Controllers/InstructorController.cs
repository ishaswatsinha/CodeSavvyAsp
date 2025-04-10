using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class InstructorController : Controller
    {
        private readonly AppDbContext _context;

        public InstructorController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Instructor Dashboard (My Profile)
        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("InstructorEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);

            if (instructor == null)
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            return View(instructor);
        }

        // ✅ Placeholder for instructor's course list
        public IActionResult MyCourses()
        {
            return View();
        }

        // ✅ Edit by logged-in instructor (from session)
        [HttpGet]
        public IActionResult Edit()
        {
            var email = HttpContext.Session.GetString("InstructorEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ILogin", "InstructorLogin");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Email == email);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor); // Edit.cshtml
        }

        // ✅ Edit by specific ID (admin use or deep link)
        [HttpGet]
        public IActionResult EditById(int id)
        {
            var instructor = _context.Instructors.FirstOrDefault(i => i.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            // Optional session update for smoother experience
            HttpContext.Session.SetString("InstructorEmail", instructor.Email);

            return View("Edit", instructor); // Still loading Edit.cshtml
        }

        // ✅ POST: Save updated instructor info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Instructor updatedInstructor)
        {
            if (ModelState.IsValid)
            {
                var instructor = _context.Instructors.FirstOrDefault(i => i.Id == updatedInstructor.Id);
                if (instructor == null)
                {
                    return NotFound();
                }

                // Update properties
                instructor.FirstName = updatedInstructor.FirstName;
                instructor.LastName = updatedInstructor.LastName;
                instructor.Email = updatedInstructor.Email;
                instructor.PhoneNumber = updatedInstructor.PhoneNumber;
                instructor.CountryCode = updatedInstructor.CountryCode;
              //  instructor.Password = updatedInstructor.Password;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Profile updated successfully!";

                // Redirect back to Edit view to show message
                return RedirectToAction("Index");
            }

            // Validation failed, return back with input
            return View(updatedInstructor);
        }
    }
}
