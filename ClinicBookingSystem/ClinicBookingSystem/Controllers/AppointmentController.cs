/*
 * FILE             : AppointmentController.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Manages appointment creation, editing, deletion,
 *                    and dashboard views for Admin and Patient roles.
 */

using ClinicBookingSystem.Database;
using ClinicBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    public class AppointmentController : Controller
    {
        // Database context for accessing Appointment and Document tables
        private readonly ApplicationDbContext db;

        /*
         * FUNCTION     : AppointmentController (Constructor)
         * DESCRIPTION  : Initializes controller with database context.
         */
        public AppointmentController(ApplicationDbContext context)
        {
            db = context;
        }

        /*
         * FUNCTION     : AdminDashboard
         * DESCRIPTION  : Displays all appointments for admin users.
         *                Includes uploaded documents list.
         */
        public IActionResult AdminDashboard()
        {
            IActionResult result;

            // Retrieve role stored in session
            string role = HttpContext.Session.GetString("Role");

             // SECURITY CHECK (Only allow admin users)
            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Role = role;
                ViewBag.HideNavbar = false;

                // Retrieve appointments sorted by date and time
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

        /*
         * FUNCTION     : AddAppointment
         * DESCRIPTION  : Adds new appointment to database.
         *                Prevents duplicate doctor bookings.
         */
        [HttpPost]
        public IActionResult AddAppointment(Appointment a)
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            //Security Check
            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                // Model Validation
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
                    //Prevent Double Booking
                    Appointment existing = db.Appointments.FirstOrDefault(x => x.DoctorName == a.DoctorName && x.Date == a.Date && x.Time == a.Time);

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
                        // Set default appointment status
                        a.Status = "Scheduled";

                        // Save appointment to database
                        db.Appointments.Add(a);

                        db.SaveChanges();

                        // Log appointment creation
                        Console.WriteLine("LOG: Appointment created for " + a.PatientName
                        + " with " + a.DoctorName + " on " + a.Date + " at " + a.Time);

                        result = RedirectToAction("AdminDashboard");
                    }
                }
            }

            return result;
        }

        /*
         * FUNCTION     : RemoveAppointment
         * DESCRIPTION  : Deletes appointment by ID.
         */
        [HttpPost]
        public IActionResult RemoveAppointment(int id)
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            //Security Check
            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                // Find appointment by ID
                Appointment a = db.Appointments.FirstOrDefault(x => x.Id == id);

                if (a != null)
                {
                    // Remove appointment from database
                    db.Appointments.Remove(a);

                    db.SaveChanges();

                    Console.WriteLine("LOG: Appointment deleted ID = " + id);
                }

                result = RedirectToAction("AdminDashboard");
            }

            return result;
        }

        /*
        * FUNCTION     : Edit (GET)
        * DESCRIPTION  : Displays edit form for selected appointment.
        */
        public IActionResult Edit(int id)
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                // Retrieve appointment by ID
                Appointment a = db.Appointments.FirstOrDefault(x => x.Id == id);

                ViewBag.Role = role;

                result = View(a);
            }

            return result;
        }

        /*
         * FUNCTION     : Edit (POST)
         * DESCRIPTION  : Updates appointment details.
         */
        [HttpPost]
        public IActionResult Edit(Appointment a)
        {
            IActionResult result;

            // Find existing appointment
            Appointment existing = db.Appointments.FirstOrDefault(x => x.Id == a.Id);

            if (existing != null)
            {
                // Update appointment fields
                existing.PatientName = a.PatientName;
                existing.DoctorName = a.DoctorName;
                existing.Date = a.Date;
                existing.Time = a.Time;
                existing.Reason = a.Reason;

                db.SaveChanges();
            }

            result = RedirectToAction("AdminDashboard");

            return result;
        }

        /*
        * FUNCTION     : PatientDashboard
        * DESCRIPTION  : Displays appointments filtered by search or date.
        */
        public IActionResult PatientDashboard(string search, DateTime? date)
        {
            IActionResult result;

            string role = HttpContext.Session.GetString("Role");

            //Security Check
            if (role == null)
            {
                result = RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Role = role;

                // Build query dynamically
                var appointments = db.Appointments.AsQueryable();

                //Search Filter
                if (!string.IsNullOrEmpty(search))
                {
                    appointments = appointments.Where(a =>
                        a.PatientName.Contains(search) ||
                        a.DoctorName.Contains(search));
                }

                //Date Filter
                if (date != null)
                {
                    appointments = appointments.Where(a =>
                        a.Date == date);
                }

                result = View(appointments
                    .OrderBy(a => a.Date)
                    .ToList());
            }

            return result;
        }
    }
}
