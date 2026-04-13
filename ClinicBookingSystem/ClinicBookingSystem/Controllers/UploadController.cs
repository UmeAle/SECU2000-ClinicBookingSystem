/*
 * FILE			         : UploadController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace ClinicBookingSystem.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Role = "Admin";

            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);

                FileStream stream = new FileStream(path, FileMode.Create);

                file.CopyTo(stream);

                stream.Close();
            }

            return RedirectToAction("Index");
        }
    }
}
