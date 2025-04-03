using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
