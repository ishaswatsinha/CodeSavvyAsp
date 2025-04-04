using CodeSavvyAsp.net.Data;
using CodeSavvyAsp.net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodeSavvyAsp.net.Controllers
{
    public class SLogin : Controller
    {

        private readonly AppDbContext _context;

        public SLogin(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login(string Email , string Password)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == Email && s.Password == Password);
            if ( student!= null)
            {
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index","SAcountDetails");
            }

            else
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
        }
        public IActionResult Signup(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                int rowsAffected = _context.SaveChanges();

                Console.WriteLine("Rows inserted" + rowsAffected);
                if (rowsAffected > 0)
                {
                    TempData["SuccessMessage"] = "Account created Succesfully";
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

        public IActionResult Forgot()
        {
            return View();
        }
        
        public IActionResult ResendEmail()
        {
            return View();
        }
        public IActionResult ChooseNewPassword()
        {
            return View();
        }
        public IActionResult ResetComplete()
        {
            return View();
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }


    }
}
