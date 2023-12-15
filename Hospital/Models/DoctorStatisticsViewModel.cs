namespace Hospital.Models
{
    public class DoctorStatisticsViewModel
    {
        public Doctor Doctor { get; set; }
        public List<(DateTime StartTime, DateTime EndTime)> DutyHours { get; set; }
        public int NumberOfVisits { get; set; }
    }
}
