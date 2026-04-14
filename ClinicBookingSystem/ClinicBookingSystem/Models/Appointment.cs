/*
 * FILE             : Appointment.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Stores appointment information for patients
 *                    and doctors.
 */


using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Appointment
    {
        // Unique identifier for appointment
        public int Id { get; set; }

        // Name of patient booking the appointment
        [Required]
        public string PatientName { get; set; }

        // Name of doctor assigned to appointment
        [Required]
        public string DoctorName { get; set; }

        // Appointment date
        [Required]
        public DateTime Date { get; set; }

        // Appointment time
        [Required]
        public string Time { get; set; }

        // Reason for appointment visit
        [Required]
        public string Reason { get; set; }

        // Appointment status (Scheduled, Cancelled, Completed)
        public string? Status { get; set; }
    }
}
