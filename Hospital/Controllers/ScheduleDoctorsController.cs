using Hospital.Data;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ScheduleDoctorsController : Controller
{

    private readonly HospitalContext _context;
    public ScheduleDoctorsController(HospitalContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var timeSlots = GetCurrentWeekTimeSlots();

        var viewModel = new ScheduleDoctorsViewModel
        {
            TimeSlots = timeSlots,
            Doctors = _context.Doctor.ToList()
        };

        return View(viewModel);
    }
    public IActionResult CopyScheduleFromPreviousWeek()
    {
        Hospital.Models.DayOfWeek currentDayOfWeek = DateTime.Now.DayOfWeek - 1 >= 0 ? (Hospital.Models.DayOfWeek)(int)(DateTime.Now.DayOfWeek - 1) : Hospital.Models.DayOfWeek.Sunday;
        DateTime currentWeekStartDate = DateTime.Today.AddDays(-(int)currentDayOfWeek);
        DateTime previousWeekStartDate = currentWeekStartDate.AddDays(-7);

        var previousWeekSchedule = _context.TimeSlot
            .Include(ts => ts.DoctorOnDuty)
            .Where(ts => ts.Date >= previousWeekStartDate && ts.Date < currentWeekStartDate)
            .ToList();

        foreach (var timeSlot in previousWeekSchedule)
        {
            var newTimeSlot = new TimeSlot
            {
                DoctorOnDuty = timeSlot.DoctorOnDuty,
                Date = timeSlot.Date.AddDays(7), 
                StartTime = timeSlot.StartTime.AddDays(7),
                EndTime = timeSlot.EndTime.AddDays(7),
                DayOfWeek = timeSlot.DayOfWeek,
            };

            _context.TimeSlot.Add(newTimeSlot);
        }

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult View(Hospital.Models.DayOfWeek dayOfWeek, DateTime date, int hour)
    {
        var uniqueDoctorNames = _context.TimeSlot
            .Where(ts => ts.StartTime.Hour == hour && ts.DoctorOnDuty != null && ts.Date == date)
            .Select(ts => ts.DoctorOnDuty.Name)
            .Distinct()
            .ToList();

        var concatenatedNames = string.Join(", ", uniqueDoctorNames);

        var viewModel = new ViewDoctorsViewModel
        {
            Doctors = concatenatedNames,
            SelectedDate = date.ToString("d"),
            StartHour = hour + ":00",
            EndHour = hour + 1 + ":00",
            DayOfWeek = dayOfWeek
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Schedule(int doctorId, Hospital.Models.DayOfWeek dayOfWeek, int hour)
    {
        var doctor = _context.Doctor.Find(doctorId);

        if (doctor != null)
        {
            var selectedDate = DateTime.Now.Date;
            var startDate = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)dayOfWeek);
            var selectedHour = startDate.AddHours(hour);

            var timeSlot = _context.TimeSlot
                .FirstOrDefault(ts => ts.DayOfWeek == dayOfWeek && ts.StartTime.Hour == hour && ts.DoctorOnDuty == doctor);

            if (timeSlot == null)
            {
                for (int i = 0; i < 4; i++)
                {
                    var startTime = startDate.AddMinutes(i * 15).AddHours(hour);
                    var endTime = startTime.AddMinutes(15);

                    timeSlot = new TimeSlot
                    {
                        Date = startDate,
                        EndTime = endTime,
                        StartTime = startTime,
                        DayOfWeek = (Hospital.Models.DayOfWeek)startTime.DayOfWeek
                    };
                    timeSlot.DoctorOnDuty = doctor;
                    _context.TimeSlot.Add(timeSlot);
                }

                _context.TimeSlot.Add(timeSlot);
            }
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
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
