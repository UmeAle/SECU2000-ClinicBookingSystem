/*
 * FILE             : User.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Stores user login credentials and role information.
 */

using System.Globalization;

namespace ClinicBookingSystem.Models
{
    public class User
    {
        // Unique identifier for user
        public int Id { get; set; }

        // User login email
        public string Email { get; set; }

        // User password (should be stored as hashed value)
        public string Password { get; set; }

        // User role (Admin or Patient)
        public string Role { get; set; }

    }
}
