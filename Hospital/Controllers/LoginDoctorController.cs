using Hospital.Models;
using Hospital.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class LoginDoctorController : Controller
    {
        private readonly DoctorService _doctorService;

        public LoginDoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Doctor user)
        {
            if (_doctorService.IsValidUser(user.Username, user.Password))
            {
                // Successful login, redirect to home or another page
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(user);
        }
    }
}
