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
        var currentWeekTimeSlots = GetCurrentWeekTimeSlots();

        if (currentWeekTimeSlots is null || currentWeekTimeSlots.Count == 0)
        {
            GenerateTimeSlotsForCurrentWeek();
            currentWeekTimeSlots = GetCurrentWeekTimeSlots();
        }

        var viewModel = new ScheduleViewModel
        {
            TimeSlots = currentWeekTimeSlots,
            Doctors = _context.Doctor.ToList(),
            Patients = _context.Patient.ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Schedule(int timeSlotId, int doctorId, int patientId)
    {
        var timeSlot = _context.TimeSlot.Find(timeSlotId);

        if (timeSlot != null && timeSlot.Patient == null)
        {
            var doctor = _context.Doctor.Find(doctorId);
            var patient = _context.Patient.Find(patientId);

            timeSlot.DoctorOnDuty = doctor;
            timeSlot.Patient = patient;
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public List<TimeSlot> GetTimeSlotsForDay(Hospital.Models.DayOfWeek dayOfWeek)
    {
        var currentDate = DateTime.Now.Date;
        var startDate = currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)Hospital.Models.DayOfWeek.Monday);
        var endDate = startDate.AddDays(7);

        return _context.TimeSlot
            .Where(ts => ts.StartTime >= startDate && ts.StartTime < endDate && ts.DayOfWeek == dayOfWeek)
            .OrderBy(ts => ts.StartTime)
            .ToList();
    }

    private void GenerateTimeSlotsForCurrentWeek()
    {
        var currentDate = DateTime.Now.Date;
        var startDate = currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)Hospital.Models.DayOfWeek.Monday);
        var endDate = startDate.AddDays(7);

        while (startDate < endDate)
        {
            for (int i = 0; i < 96; i++)
            {
                var startTime = startDate.AddMinutes(i * 15);
                var endTime = startTime.AddMinutes(15);

                var timeSlot = new TimeSlot
                {
                    Date = startDate,
                    EndTime = endTime,
                    StartTime = startTime,
                    DayOfWeek = (Hospital.Models.DayOfWeek)startTime.DayOfWeek
                };

                _context.TimeSlot.Add(timeSlot);
            }

            startDate = startDate.AddDays(1);
        }

        _context.SaveChanges();
    }

    private List<TimeSlot> GetCurrentWeekTimeSlots()
    {
        var currentDate = DateTime.Now.Date;
        var startDate = currentDate.AddDays(-(int)(Hospital.Models.DayOfWeek)currentDate.DayOfWeek + (int)Hospital.Models.DayOfWeek.Monday);
        var endDate = startDate.AddDays(7);

        var currentWeekTimeSlots = _context.TimeSlot
            .Where(ts => ts.StartTime >= startDate && ts.StartTime < endDate)
            .OrderBy(ts => ts.DayOfWeek)
            .ThenBy(ts => ts.StartTime)
            .ToList();

        return currentWeekTimeSlots;
    }
}
