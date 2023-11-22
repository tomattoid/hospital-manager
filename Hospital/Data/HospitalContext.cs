using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hospital.Models;

namespace Hospital.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext (DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public DbSet<Hospital.Models.Doctor> Doctor { get; set; } = default!;

        public DbSet<Hospital.Models.Patient> Patient { get; set; } = default!;

        public DbSet<Hospital.Models.TimeSlot> TimeSlot { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Doctor>()
                .Property(e => e.Specialty)
                .HasConversion(
                    v => v.ToString(),
                    v => (Spec)Enum.Parse(typeof(Spec), v));
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Username)
                .IsUnique();
            modelBuilder.Entity<Patient>()
                .HasIndex(d => d.Username)
                .IsUnique();
        }
    }
}
