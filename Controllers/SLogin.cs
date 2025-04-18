using CodeSavvyAsp.Data;
using CodeSavvyAsp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class SLogin : Controller
    {
        private readonly AppDbContext _context;

        public SLogin(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == Email && s.Password == Password);
            if (student != null)
            {
                // ✅ Session Set Ho Raha Hai
                HttpContext.Session.SetString("StudentEmail", student.Email);
                HttpContext.Session.SetString("StudentName", student.FirstName);

                TempData["SuccessMessage"] = "Login successful!";

                // ✅ Redirect to SAccountDetails Controller
                return RedirectToAction("Index", "SAccountDetails");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                int rowsAffected = _context.SaveChanges();

                Console.WriteLine("Rows inserted: " + rowsAffected);
                if (rowsAffected > 0)
                {
                    TempData["SuccessMessage"] = "Account created successfully!";
                    return RedirectToAction("Login");
                }
                else
                {
                    Console.WriteLine("⚠ Data was not inserted!");
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            return View(student);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // ✅ Session Clear
            return RedirectToAction("Login", "SLogin");
        }
    }
}
