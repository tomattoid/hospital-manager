using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Hospital.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
