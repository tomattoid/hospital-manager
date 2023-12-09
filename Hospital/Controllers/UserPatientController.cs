using Hospital.Data;
using Hospital.Models;
using Hospital.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class UserPatientController : Controller
    {
        private readonly HospitalContext _context;
        private readonly IAccountService _accountService;
        public UserPatientController(HospitalContext context, IAccountService accountService)
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
                .Where(ts => ts.Patient.Id == _accountService.PatientId)
                .Select(ts => new ShowVisitsViewModel
                {
                    Date = ts.Date,
                    Time = ts.StartTime.ToString("hh:mm tt") + " - " + ts.EndTime.ToString("hh:mm tt"),
                    PersonName = ts.DoctorOnDuty.Name,
                    Description = ts.Description
                })
                .ToList();

            return View(reservations);
        }
    
    public IActionResult ReserveVisit()
        {
            var availableTimeSlots = _context.TimeSlot
                .Include(ts => ts.DoctorOnDuty)
                .Where(ts => ts.Patient == null)
                .ToList();

            var specialties = Enum.GetValues(typeof(Spec))
                .Cast<Spec>()
                .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() })
                .ToList();

            var viewModel = new ReserveVisitViewModel
            {
                AvailableTimeSlots = availableTimeSlots,
                Specialties = specialties,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ReserveVisit(ReserveVisitViewModel viewModel)
        {
            // Handle the search based on the selected specialty
            var filteredTimeSlots = _context.TimeSlot
                .Include(ts => ts.DoctorOnDuty)
                .Where(ts => ts.Patient == null &&
                             (!viewModel.SelectedSpecialty.HasValue || ts.DoctorOnDuty.Specialty == viewModel.SelectedSpecialty))
                .ToList();

            viewModel.AvailableTimeSlots = filteredTimeSlots;

            // Populate specialties again for the dropdown
            viewModel.Specialties = Enum.GetValues(typeof(Spec))
                .Cast<Spec>()
                .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() })
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ReserveTimeSlot(int timeSlotId)
        {
            var timeSlot = _context.TimeSlot.Find(timeSlotId);

            if (timeSlot != null && timeSlot.Patient == null)
            {
                // Set the time slot as reserved and associate it with the logged-in patient
                var loggedInPatient = _context.Patient.FirstOrDefault(p => p.Id == _accountService.PatientId);

                if (loggedInPatient != null)
                {
                    timeSlot.Patient = loggedInPatient;

                    _context.SaveChanges();

                    // Optionally, redirect to a confirmation page or refresh the view
                    return RedirectToAction("ReserveVisit");
                }
            }

            // Handle the case where the reservation fails (e.g., time slot not available)
            // You can add error messages or redirect to an error page as needed

            return RedirectToAction("ReserveVisit");
        }
    }
}
