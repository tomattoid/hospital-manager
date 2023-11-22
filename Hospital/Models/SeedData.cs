using Hospital.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HospitalContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<HospitalContext>>()))
            {
                if (context.Doctor.Any())
                {
                    return;
                }

                context.Doctor.AddRange(
                    new Doctor
                    {
                        Name = "John Smith",
                        Username = "johnsmith",
                        Password = "12345678",
                        Specialty = Spec.FamilyDoctor,
                        IsAdmin = true
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
