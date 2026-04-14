/*
 * FILE			         : UploadController.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

using Microsoft.AspNetCore.Mvc;
using System.IO;
using ClinicBookingSystem.Models;
using ClinicBookingSystem.Database;

namespace ClinicBookingSystem.Controllers
{
    public class UploadController : Controller
    {

        private readonly ApplicationDbContext db;

        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Role = role;

            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (file == null || file.Length == 0)
            {
                return RedirectToAction("Index");
            }

            // allow only safe file types
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".pdf" && extension != ".png" && extension != ".jpg")
            {
                ViewBag.Error = "Invalid file type";
                return View("Index");
            }

            string fileName = Path.GetFileName(file.FileName);

            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/files",
                fileName
            );

            FileStream stream = new FileStream(path, FileMode.Create);

            file.CopyTo(stream);

            stream.Close();

            // save to database (audit trail)
            Document doc = new Document();

            doc.FileName = fileName;
            doc.FilePath = "/files/" + fileName;

            db.Documents.Add(doc);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
