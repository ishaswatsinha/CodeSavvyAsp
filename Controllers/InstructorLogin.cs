using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class InstructorLogin : Controller
    {
        public IActionResult InstructorIndex()
        {
            return View();
        }

        public IActionResult ISignup()
        {
            return View();
        }

        public IActionResult VerifyPhone()
        {
            return View();
        }


        public IActionResult ForgotPassIns()
        {
            return View();
        }
    }
}
