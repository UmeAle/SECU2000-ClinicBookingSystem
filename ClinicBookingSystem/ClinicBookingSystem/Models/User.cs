/*
 * FILE			         : User.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia, Connar Thompson, Jobair Ahmed Jisan
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using System.Globalization;

namespace ClinicBookingSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Usernmae { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
