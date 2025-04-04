using Microsoft.AspNetCore.Mvc;

namespace CodeSavvyAsp.net.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
