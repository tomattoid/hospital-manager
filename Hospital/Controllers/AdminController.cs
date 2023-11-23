using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
