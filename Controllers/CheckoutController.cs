using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuyNow(int courseId)
        {
          
            ViewBag.CourseId = courseId;
            return View("PurchaseConfirmation"); // ye view banana padega
        }
    }
}
