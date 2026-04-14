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
using System.Text;
using System.Security.Cryptography;

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

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes =
                    sha256.ComputeHash(
                        Encoding.UTF8.GetBytes(password));

                return Convert.ToBase64String(bytes);
            }
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            IActionResult result;

            // check if user exists in database
            User user = db.Users.FirstOrDefault(u => u.Email == username);

            if (user == null || user.Password != password)
            {
                ViewBag.Error = "Incorrect email or password";
                ViewData["HideNavBar"] = true;

                Console.WriteLine(HashPassword("myPassword123"));
                Console.WriteLine(HashPassword("testing123"));
                Console.WriteLine(HashPassword("testing456"));

                result = View();
            }
            else
            {
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    result = RedirectToAction("AdminDashboard", "Appointment");
                }
                else
                {
                    result = RedirectToAction("PatientDashboard", "Appointment");
                }
            }
            return result;
        }
    }
}
