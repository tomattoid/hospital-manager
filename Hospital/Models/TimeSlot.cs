using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public Doctor DoctorOnDuty { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Patient? Patient { get; set; }
    }
}
