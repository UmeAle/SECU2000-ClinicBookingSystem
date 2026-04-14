/*
 * FILE			         : AccountController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia, Connar Thompson
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
            // prevent empty login
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                ViewBag.HideNavbar = true;

                return View();
            }

            // check if user exists
            User user = db.Users.FirstOrDefault(u => u.Email == username);

            // create user if first time logging in
            if (user == null)
            {
                user = new User();

                user.Email = username;
                user.Password = password;

                // assign role
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

            // check password
            if (user.Password != password)
            {
                ViewBag.Error = "Invalid password";
                ViewBag.HideNavbar = true;

                return View();
            }

            // store role for navbar + security
            HttpContext.Session.SetString("Role", user.Role);

            // redirect based on role
            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Appointment");
            }
            else
            {
                return RedirectToAction("PatientDashboard", "Appointment");
            }
        }
    }
}
