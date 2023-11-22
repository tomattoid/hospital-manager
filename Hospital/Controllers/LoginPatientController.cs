using Hospital.Models;
using Hospital.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class LoginPatientController : Controller
    {
        private readonly UserService _userService;

        public LoginPatientController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Patient user)
        {
            if (_userService.IsValidUser(user.Username, user.Password))
            {
                // Successful login, redirect to home or another page
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(user);
        }
    }
}
