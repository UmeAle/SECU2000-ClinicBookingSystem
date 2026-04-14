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

                List<Appointment> appointments = db.Appointments
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.Time)
                    .ToList();

                List<Document> docs = db.Documents.ToList();

                ViewBag.Documents = docs;


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
                    Appointment existing = db.Appointments.FirstOrDefault(x => x.DoctorName == a.DoctorName &&
                                           x.Date == a.Date && x.Time == a.Time);

                    if (existing != null)
                    {
                        ViewBag.Error = "Doctor already has an appointment at that time.";

                        ViewBag.Role = role;
                        ViewBag.HideNavbar = false;

                        List<Appointment> list = db.Appointments
                            .OrderBy(x => x.Date)
                            .ThenBy(x => x.Time)
                            .ToList();

                        result = View("AdminDashboard", list);
                    }
                    else
                    {
                        a.Status = "Scheduled";

                        db.Appointments.Add(a);

                        db.SaveChanges();

                        Console.WriteLine("LOG: Appointment created for " + a.PatientName
                        + " with " + a.DoctorName + " on " + a.Date + " at " + a.Time);

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

                    Console.WriteLine("LOG: Appointment deleted ID = " + id);
                }

                result = RedirectToAction("AdminDashboard");
            }

            return result;
        }

        public IActionResult Edit(int id)
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            Appointment a = db.Appointments.FirstOrDefault(x => x.Id == id);

            ViewBag.Role = role;
            ViewBag.HideNavbar = false;

            return View(a);
        }


        [HttpPost]
        public IActionResult Edit(Appointment a)
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            Appointment existing = db.Appointments.FirstOrDefault(x => x.Id == a.Id);

            if (existing != null)
            {
                existing.PatientName = a.PatientName;
                existing.DoctorName = a.DoctorName;
                existing.Date = a.Date;
                existing.Time = a.Time;
                existing.Reason = a.Reason;

                db.SaveChanges();

                Console.WriteLine("LOG: Appointment updated ID = " + a.Id);
            }

            return RedirectToAction("AdminDashboard");
        }

        public IActionResult PatientDashboard(string search)
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

                List<Appointment> appointments = db.Appointments

                    .Where(a =>
                        search == null ||
                        a.PatientName.Contains(search) ||
                        a.DoctorName.Contains(search) ||
                        a.Reason.Contains(search)
                    )

                    .OrderBy(a => a.Date)
                    .ThenBy(a => a.Time)

                    .ToList();

                result = View(appointments);
            }

            return result;
        }
    }
}
