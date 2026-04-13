/*
 * FILE			         : ApplicationDbContext.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia, Connar Thompson, Jobair Ahmed Jisan
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using Microsoft.EntityFrameworkCore;
using ClinicBookingSystem.Models;

namespace ClinicBookingSystem.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users{get; set;}
        public DbSet<Appointment> Appointments { get; set;} 
        public DbSet<Document> Documents { get; set;}

    }
}
