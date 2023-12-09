namespace Hospital.Models
{
    public class ViewDoctorsViewModel
    {
        public string Doctors { get; set; }
        public string SelectedDate { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public Hospital.Models.DayOfWeek DayOfWeek { get; set; }
    }
}
