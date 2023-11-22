using Hospital.Data;
using Hospital.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class AccountController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly UserService _userService;
        private readonly HospitalContext _context;

        public AccountController(DoctorService doctorService, UserService userService, HospitalContext context)
        {
            _doctorService = doctorService;
            _userService = userService;
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginPatient()
        {
            return View();
        }
        public IActionResult LoginDoctor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginDoctor(Doctor doctor)
        {
            if (_doctorService.IsValidUser(doctor.Username, doctor.Password))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(doctor);
        }
        [HttpPost]
        public IActionResult LoginPatient(Patient patient)
        {
            if (_doctorService.IsValidUser(patient.Username, patient.Password))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(patient);
        }

        [HttpGet]
        public IActionResult RegisterPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterPatient(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password,
                };

                _context.Patient.Add(patient);
                _context.SaveChanges();

                return RedirectToAction("LoginPatient");
            }

            return View(model);
        }
    }
}
