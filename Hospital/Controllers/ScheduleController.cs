using Hospital.Data;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

public class ScheduleController : Controller
{
    private readonly HospitalContext _context;

    public ScheduleController(HospitalContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (!_context.TimeSlot.Any())
        {
            GenerateTimeSlotsForCurrentWeek();
        }

        var currentWeekTimeSlots = GetCurrentWeekTimeSlots();

        var doctors = _context.Doctor.ToList();
        var patients = _context.Patient.ToList();

        var viewModel = new ScheduleViewModel
        {
            TimeSlots = currentWeekTimeSlots,
            Doctors = doctors,
            Patients = patients
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Schedule(int timeSlotId, int doctorId, int patientId)
    {
        var timeSlot = _context.TimeSlot.Find(timeSlotId);

        if (timeSlot != null && timeSlot.IsAvailable)
        {
            var doctor = _context.Doctor.Find(doctorId);
            var patient = _context.Patient.Find(patientId);

            timeSlot.DoctorOnDuty = doctor;
            timeSlot.Patient = patient;
            timeSlot.IsAvailable = false;

            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    private void GenerateTimeSlotsForCurrentWeek()
    {
        var currentDate = DateTime.Now.Date;
        var endDate = currentDate.AddDays(7);

        while (currentDate < endDate)
        {
            for (int i = 0; i < 96; i++)
            {
                var startTime = currentDate.AddMinutes(i * 15);
                var endTime = startTime.AddMinutes(15);

                var timeSlot = new TimeSlot
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    IsAvailable = true,
                    Date = currentDate
                };

                _context.TimeSlot.Add(timeSlot);
            }

            currentDate = currentDate.AddDays(1);
        }

        _context.SaveChanges();
    }

    private List<TimeSlot> GetCurrentWeekTimeSlots()
    {
        var currentDate = DateTime.Now.Date;
        var endDate = currentDate.AddDays(7);

        return _context.TimeSlot
            .Where(ts => ts.StartTime >= currentDate && ts.StartTime < endDate)
            .ToList();
    }
}
