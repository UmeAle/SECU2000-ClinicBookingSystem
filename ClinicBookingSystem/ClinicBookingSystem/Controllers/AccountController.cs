/*
 * FILE             : AccountController.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia, Connar Thompson
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Handles user authentication including login
 *                    validation and role-based navigation.
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
        // Database context used to access Users table
        private readonly ApplicationDbContext db;


        /*
         * FUNCTION     : AccountController (Constructor)
         * DESCRIPTION  : Initializes controller with database context.
         */
        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

         /*
         * FUNCTION     : Login (GET)
         * DESCRIPTION  : Displays login page.
         */
        public IActionResult Login()
        {
            // Hide navigation bar on login page
            ViewBag.HideNavbar = true;

            return View();
        }

        /*
         * FUNCTION     : HashPassword
         * DESCRIPTION  : Generates SHA256 hash for password.
         */
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert password string to byte array
                byte[] bytes =
                    sha256.ComputeHash(
                        Encoding.UTF8.GetBytes(password));

                // Convert hash bytes to base64 string
                return Convert.ToBase64String(bytes);
            }
        }

        /*
        * FUNCTION     : Login (POST)
        * DESCRIPTION  : Validates user credentials and redirects
        *                to appropriate dashboard based on role.
        */
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            IActionResult result;

            // Retrieve user from database using email
            User user = db.Users.FirstOrDefault(u => u.Email == username);

            // Check if user exists and password matches
            if (user == null || user.Password != password)
            {
                ViewBag.Error = "Incorrect email or password";
                ViewData["HideNavBar"] = true;

                result = View();
            }
            else
            {
                // Store user role in session
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    // Redirect admin users to admin dashboard
                    result = RedirectToAction("AdminDashboard", "Appointment");
                }
                else
                {
                    // Redirect patients to patient dashboard
                    result = RedirectToAction("PatientDashboard", "Appointment");
                }
            }
            return result;
        }
    }
}
