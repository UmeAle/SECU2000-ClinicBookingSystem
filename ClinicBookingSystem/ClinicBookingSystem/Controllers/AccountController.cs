/*
 * FILE			         : AccountController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using Microsoft.AspNetCore.Mvc;
using ClinicBookingSystem.Database;
using ClinicBookingSystem.Models;
using System.Linq;

namespace ClinicBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;

        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Login()
        {

            ViewBag.HideNavbar = true;

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // VALIDATION: prevent empty login
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                ViewData["HideNavbar"] = true;

                return View();
            }

            User user = db.Users.FirstOrDefault(u => u.Email == username);

            // if user doesn't exist → create new patient/admin
            if (user == null)
            {
                user = new User();

                user.Email = username;
                user.Password = password;

                if (username == "admin@clinic.com")
                {
                    user.Role = "Admin";
                }
                else
                {
                    user.Role = "Patient";
                }

                db.Users.Add(user);
                db.SaveChanges();
            }

            ViewBag.Role = user.Role;

            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Appointment");
            }

            return RedirectToAction("PatientDashboard", "Appointment");
        }
    }
}
