/*
 * FILE			         : AppointmentController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using Microsoft.AspNetCore.Mvc;
using ClinicBookingSystem.Models;

namespace ClinicBookingSystem.Controllers
{
    public class AppointmentController : Controller
    {
      
        static List<Appointment> appointments = new List<Appointment>();
        public IActionResult AdminDashboard()
        {
            ViewBag.Role = "Admin";

            return View(appointments);
        }

        [HttpPost]
        public IActionResult AddAppointment(Appointment a)
        {
            a.Id = appointments.Count + 1;

            a.Status = "Scheduled";

            appointments.Add(a);

            return RedirectToAction("AdminDashboard");
        }

        public IActionResult PatientDashboard()
        {
            ViewBag.Role = "Patient";
            return View(appointments);
        }
    }
}
