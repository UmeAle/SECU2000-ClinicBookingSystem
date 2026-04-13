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
            User user = db.Users.FirstOrDefault(u => u.Email == username);

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

                //update password in case user typed a new one
                user.Password = password;

                if(user.Id ==0)
                {
                    db.Users.Add(user);
                }

                db.SaveChanges();
            }

            if (username == "Admin")
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
