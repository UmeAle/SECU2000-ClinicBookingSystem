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


            //LOGIN CHECKER
            bool passCheck = false;
            foreach (User use in db.Users)
            {
                if (use.Email == username && !passCheck)
                {
                    if (use.Password == password)
                    {
                        passCheck = true;
                    }
                }
            }

            if (passCheck)
            {
                if (username == "Admin")
                {
                    return RedirectToAction("AdminDashboard", "Appointment");
                }
                else
                {
                    return RedirectToAction("PatientDashboard", "Appointment");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account"); 
            }
        }
    }
}
