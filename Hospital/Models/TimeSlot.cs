using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public Doctor? DoctorOnDuty { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required, DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [Required, DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public Patient? Patient { get; set; }
        public bool IsAvailable { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
