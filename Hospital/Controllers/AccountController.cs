using Hospital.Data;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Hospital.Controllers
{
    public class AccountController : Controller
    {
        private readonly HospitalContext _context;

        public AccountController(HospitalContext context)
        {
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
            var user = _context.Doctor.FirstOrDefault(u => u.Username == doctor.Username);
            if (user != null && doctor.Password == user.Password)
            {
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(doctor);
        }
        [HttpPost]
        public IActionResult LoginPatient(Patient patient)
        {
            var user = _context.Patient.FirstOrDefault(u => u.Username == patient.Username);
            if (user != null && patient.Password == user.Password)
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
