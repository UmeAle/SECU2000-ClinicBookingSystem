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

            a.PatientId = 0;//NEED TO GET USER ID FOR THIS

            a.PatientName = "Name"; //NEED PATIENT NAME

            a.DoctorName = "Dr. Ligma";

            a.Date = DateTime.Now.Date; //Date entered.

            a.Time = "12:00 AM"; //TIME ENTERED

            a.Reason = "Bad Breath";

            a.Status = "Scheduled";

            appointments.Add(a);

            return RedirectToAction("AdminDashboard");
        }


        public IActionResult RemoveAppointment(Appointment a)
        {
            foreach (Appointment app in appointments)
            {
                if (a.Id == app.Id)
                {
                    appointments.Remove(app);
                }
            }
            return RedirectToAction("AdminDashboard");
        }


        public IActionResult PatientDashboard()
        {
            ViewBag.Role = "Patient";
            return View(appointments);
        }
    }
}
