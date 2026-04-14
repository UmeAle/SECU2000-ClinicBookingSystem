/*
 * FILE             : ApplicationDbContext.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia, Connar Thompson
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Defines database tables and manages
 *                    connection using Entity Framework.
 */

using Microsoft.EntityFrameworkCore;
using ClinicBookingSystem.Models;

namespace ClinicBookingSystem.Database
{
    public class ApplicationDbContext : DbContext
    {
        /*
        * FUNCTION     : ApplicationDbContext (Constructor)
        * DESCRIPTION  : Initializes database context with configuration options.
        */
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Table storing user login information
        public DbSet<User> Users{get; set;}

        // Table storing appointment records
        public DbSet<Appointment> Appointments { get; set;}

        // Table storing uploaded document metadata
        public DbSet<Document> Documents { get; set;}

    }
}
