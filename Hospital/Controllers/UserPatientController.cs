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
            var filteredTimeSlots = _context.TimeSlot
                .Include(ts => ts.DoctorOnDuty)
                .Where(ts => ts.Patient == null &&
                             (!viewModel.SelectedSpecialty.HasValue || ts.DoctorOnDuty.Specialty == viewModel.SelectedSpecialty))
                .ToList();

            viewModel.AvailableTimeSlots = filteredTimeSlots;

            viewModel.Specialties = Enum.GetValues(typeof(Spec))
                .Cast<Spec>()
                .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() })
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ReserveTimeSlot(int updatedTimeSlotId, string versionStr)
        {
            string[] numberStrings = versionStr.Split(' ');
            byte[] version = numberStrings.Select(s => byte.Parse(s)).ToArray();

            if (updatedTimeSlotId == null)
            {
                return RedirectToAction("ReserveVisit");
            }

            var existingTimeSlot = _context.TimeSlot.Find(updatedTimeSlotId);
            var updatedTimeSlot = new TimeSlot
            {
                StartTime = existingTimeSlot.StartTime,
                EndTime = existingTimeSlot.EndTime,
                Date = existingTimeSlot.Date,
                DoctorOnDuty = existingTimeSlot.DoctorOnDuty,
                DayOfWeek = existingTimeSlot.DayOfWeek,
                Version = version
            };

            if (existingTimeSlot == null)
            {
                return NotFound();
            }

            if (!IsConcurrencyValid(existingTimeSlot.Version, updatedTimeSlot.Version))
            {
                existingTimeSlot = _context.TimeSlot.Find(updatedTimeSlot.Id);

                ViewData["ConcurrencyError"] = "Concurrency conflict. Please resolve and try again.";

                return RedirectToAction("ReserveVisit");
            }

            var loggedInPatient = _context.Patient.FirstOrDefault(p => p.Id == _accountService.PatientId);

            if (loggedInPatient != null)
            {
                existingTimeSlot.Patient = loggedInPatient;
                existingTimeSlot.Version = updatedTimeSlot.Version;

                _context.SaveChanges();

                return RedirectToAction("ReserveVisit");
            }

            return RedirectToAction("ReserveVisit");
        }

        private static bool IsConcurrencyValid(byte[] original, byte[] incoming)
        {
            return original.SequenceEqual(incoming);
        }
    }
}
