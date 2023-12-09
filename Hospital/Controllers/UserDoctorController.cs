using Hospital.Data;
using Hospital.Models;
using Hospital.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class UserDoctorController : Controller
    {
        private readonly HospitalContext _context;
        private readonly IAccountService _accountService;

        public UserDoctorController(HospitalContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowVisits()
        {
            var reservations = _context.TimeSlot
                .Where(ts => ts.DoctorOnDuty.Id == _accountService.DoctorId && ts.Patient != null)
                .Select(ts => new ShowVisitsViewModel
                {
                    Date = ts.Date,
                    Time = ts.StartTime.ToString("hh:mm tt") + " - " + ts.EndTime.ToString("hh:mm tt"),
                    PersonName = ts.Patient.Name,
                    Description = ts.Description,
                    TimeSlotId = ts.Id
                })
                .ToList();

            return View(reservations);
        }
        [HttpGet]
        public IActionResult Edit(int TimeSlotId)
        {
            var timeSlot = _context.TimeSlot
                .Include(ts => ts.Patient)
                .SingleOrDefault(ts => ts.Id == TimeSlotId);

            if (timeSlot == null)
            {
                return NotFound();
            }

            // Map the time slot to a view model
            var reservationViewModel = new ShowVisitsViewModel
            {
                Date = timeSlot.Date,
                Time = timeSlot.StartTime.ToString("hh:mm tt") + " - " + timeSlot.EndTime.ToString("hh:mm tt"),
                PersonName = timeSlot.Patient.Name,
                Description = timeSlot.Description,
                TimeSlotId = timeSlot.Id
            };

            return View(reservationViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ShowVisitsViewModel reservationViewModel)
        {
            // Retrieve the time slot based on the date, time, and patient
            var timeSlot = _context.TimeSlot
                .SingleOrDefault(ts => ts.Id == reservationViewModel.TimeSlotId);

            if (timeSlot == null)
            {
                return NotFound();
            }

            timeSlot.Description = reservationViewModel.Description;

            _context.SaveChanges();


            return RedirectToAction("ShowVisits");
        }
    }
}
