namespace Hospital.Models
{
    public class ShowVisitsViewModel
    {
        public int TimeSlotId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string PersonName { get; set; }
        public string? Description { get; set; }
    }
}
