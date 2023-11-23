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
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required, MinLength(8)]
        public string? Password { get; set; }
        [Required]
        public Spec Specialty { get; set; }
        public bool IsAdmin { get; set; }
    }
}
