/*
 * FILE             : HomeController.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Controls navigation for home, privacy,
 *                    and error pages.
 */

using System.Diagnostics;
using ClinicBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        // Logger used to record application events and errors
        private readonly ILogger<HomeController> _logger;

        /*
         * FUNCTION     : HomeController(Constructor)
         * DESCRIPTION  : Initializes controller with logging service.
         */
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*
         * FUNCTION     : Index
         * DESCRIPTION  : Displays the home page.
         */
        public IActionResult Index()
        {
            return View();
        }

        /*
         * FUNCTION     : Privacy
         * DESCRIPTION  : Displays privacy information page.
         */
        public IActionResult Privacy()
        {
            return View();
        }

        /*
        * FUNCTION     : Error
        * DESCRIPTION  : Displays error page with request ID.
        */
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
