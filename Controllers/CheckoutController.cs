using Microsoft.AspNetCore.Mvc;
using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CodeSavvyAsp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context; // ✅ Injecting DbContext

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Show Checkout Page (if needed)
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PurchaseConfirmation()
        {
            return View();
        }

        // ✅ BuyNow Method Optimized - No Login Required
        [HttpPost]
        public async Task<IActionResult> BuyNow(int courseId)
        {
            // ✅ Check if Course Exists
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found!";
                return RedirectToAction("ErrorPage");
            }

            try
            {
                // ✅ Enroll Student Without Login Check
                var enrollment = new EnrolledCourse
                {
                    CourseId = courseId
                };

                _context.EnrolledCourses.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Course successfully purchased!";
                return RedirectToAction("PurchaseConfirmation", "Checkout"); // ✅ Direct Confirmation Page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Unexpected error occurred: {ex.Message}";
                return RedirectToAction("ErrorPage");
            }
        }

        // ✅ Error Page to Show Custom Messages
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
