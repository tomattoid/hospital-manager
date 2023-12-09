using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Models
{
    public class ReserveVisitViewModel
    {
        public List<TimeSlot> AvailableTimeSlots { get; set; }
        public List<SelectListItem> Specialties { get; set; }
        public Spec? SelectedSpecialty { get; set; }
    }
}
