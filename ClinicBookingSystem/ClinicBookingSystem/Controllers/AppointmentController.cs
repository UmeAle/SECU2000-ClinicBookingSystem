/*
 * FILE			         : AppointmentController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using ClinicBookingSystem.Database;
using ClinicBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly ApplicationDbContext db;

        public AppointmentController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult AdminDashboard()
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Role = role;

                ViewBag.HideNavbar = false;

                List<Appointment> appointments = db.Appointments.ToList();

                result = View(appointments);
            }

            return result;
        }

        [HttpPost]
        public IActionResult AddAppointment(Appointment a)
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "All fields required";

                    ViewBag.Role = role;
                    ViewBag.HideNavbar = false;

                    List<Appointment> list = db.Appointments.ToList();

                    result = View("AdminDashboard", list);
                }
                else
                {
                    // CHECK FOR DUPLICATE
                    Appointment existing = db.Appointments.FirstOrDefault(x => x.PatientName == a.PatientName &&
                        x.DoctorName == a.DoctorName && x.Date == a.Date && x.Time == a.Time);

                    if (existing != null)
                    {
                        ViewBag.Error = "Appointment already exists";

                        ViewBag.Role = role;
                        ViewBag.HideNavbar = false;

                        List<Appointment> list = db.Appointments.ToList();

                        result = View("AdminDashboard", list);
                    }
                    else
                    {
                        a.Status = "Scheduled";

                        db.Appointments.Add(a);

                        db.SaveChanges();

                        result = RedirectToAction("AdminDashboard");
                    }
                }
            }

            return result;
        }

        [HttpPost]
        public IActionResult RemoveAppointment(int id)
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                Appointment a = db.Appointments.FirstOrDefault(x => x.Id == id);

                if (a != null)
                {
                    db.Appointments.Remove(a);

                    db.SaveChanges();
                }

                result = RedirectToAction("AdminDashboard");
            }

            return result;
        }

        public IActionResult PatientDashboard()
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            if (role == null)
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Role = role;

                ViewBag.HideNavbar = false;

                List<Appointment> appointments = db.Appointments.ToList();

                result = View(appointments);
            }

            return result;
        }
    }
}
