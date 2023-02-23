using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HMS.Models.Patient> Patient { get; set; }

        public DbSet<HMS.Models.Medicine> Medicine { get; set; }

        public DbSet<HMS.Models.Visitations> Visitations { get; set; }

        public DbSet<HMS.Models.Appointment> Appointment { get; set; }
    }
}
