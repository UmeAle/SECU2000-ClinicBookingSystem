/*
 * FILE			         : AccountController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia, Connar Thompson, Jobair Ahmed Jisan
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin")
            {
                return RedirectToAction("AdminDashboard", "Appointment");
            }

            return RedirectToAction("PatientDashboard", "Appointment");
        }
    }
}
