using System.ComponentModel.DataAnnotations;

namespace Hospital.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required, MinLength(8)]
        public string? Password { get; set; }

    }
}