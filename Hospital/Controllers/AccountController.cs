using Hospital.Data;
using Hospital.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Hospital.Controllers
{
    public class AccountController : Controller
    {
        public Patient? loggedPatient { get; set;}
        public Doctor? loggedDoctor { get; set;}
        private readonly HospitalContext _context;
        private readonly IAccountService _accountService;

        public AccountController(HospitalContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
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
            if (user != null && doctor.Password == user.Password && user.IsAdmin)
            {
                loggedDoctor = user;
                _accountService.DoctorId = user.Id;
                return RedirectToAction("Index", "Admin");
            }
            else if (user != null && doctor.Password == user.Password && !doctor.IsAdmin)
            {
                loggedDoctor = user;
                _accountService.DoctorId = user.Id;
                return RedirectToAction("Index", "UserDoctor");
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
                loggedPatient = user;
                _accountService.PatientId = user.Id;
                return RedirectToAction("Index", "UserPatient");
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
