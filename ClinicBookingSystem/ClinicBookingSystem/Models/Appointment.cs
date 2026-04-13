/*
 * FILE			         : Appointment.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia, Connar Thompson, Jobair Ahmed Jisan
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */


namespace ClinicBookingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime Date { get; set; }
        public string Time {  get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

    }
}
