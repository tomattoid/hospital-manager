using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public enum Spec
    {
        FamilyDoctor,
        ThroatSpecialist,
        Dermatologist,
        Oculist,
        Neurologist,
        Ortopedist,
        Pediatrist
    }
    public class Doctor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public Spec Specialty { get; set; }
        public bool IsAdmin { get; set; }
    }
}
