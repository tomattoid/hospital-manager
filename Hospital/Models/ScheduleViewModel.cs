namespace Hospital.Models
{
    public class ScheduleViewModel
    {
        public List<TimeSlot> TimeSlots { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
