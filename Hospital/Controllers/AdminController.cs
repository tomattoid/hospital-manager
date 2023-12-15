using Hospital.Data;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class AdminController : Controller
    {
        private readonly HospitalContext _context;
        public AdminController(HospitalContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChooseRaport()
        {
            var doctors = _context.Doctor.ToList();
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult GenerateReport(int doctorId, DateTime selectedDate)
        {
            // Retrieve the selected doctor
            var selectedDoctor = _context.Doctor.Find(doctorId);

            if (selectedDoctor == null)
            {
                return NotFound();
            }

            // Retrieve duty hours for the selected doctor on the selected date
            var dutyHours = _context.TimeSlot
                .Where(ts => ts.DoctorOnDuty == selectedDoctor && ts.Date == selectedDate)
                .OrderBy(ts => ts.StartTime)
                .ToList();

            // Combine consecutive time slots into continuous ranges
            var continuousRanges = new List<(DateTime StartTime, DateTime EndTime)>();

            if (dutyHours.Count > 0)
            {
                DateTime startTime = dutyHours[0].StartTime;
                DateTime endTime = dutyHours[0].EndTime;

                for (int i = 1; i < dutyHours.Count; i++)
                {
                    if (dutyHours[i].StartTime == endTime)
                    {
                        // Consecutive time slots, update the end time
                        endTime = dutyHours[i].EndTime;
                    }
                    else
                    {
                        // Non-consecutive time slot, add the current range and start a new one
                        continuousRanges.Add((startTime, endTime));
                        startTime = dutyHours[i].StartTime;
                        endTime = dutyHours[i].EndTime;
                    }
                }

                // Add the last continuous range
                continuousRanges.Add((startTime, endTime));
            }

            // Retrieve the number of visits for the selected doctor on the selected date
            var numberOfVisits = _context.TimeSlot
                .Count(ts => ts.DoctorOnDuty == selectedDoctor && ts.Date == selectedDate && ts.Patient != null);

            // Create a ViewModel with the data
            var viewModel = new DoctorStatisticsViewModel
            {
                Doctor = selectedDoctor,
                DutyHours = continuousRanges,
                NumberOfVisits = numberOfVisits
            };

            return View(viewModel);
        }
    }
}
