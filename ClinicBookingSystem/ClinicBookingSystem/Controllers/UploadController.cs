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

        public UploadController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Role = role;

            List<Document> docs = db.Documents
                .OrderByDescending(d => d.UploadDate)
                .ToList();

            ViewBag.Documents = docs;

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


            // check file selected
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Please select a file";

                ViewBag.Documents = db.Documents.ToList();

                return View("Index");
            }


            // allowed file types
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".pdf"
                && extension != ".jpg"
                && extension != ".png")
            {
                ViewBag.Error =
                    "Invalid file type. Only PDF, JPG, PNG allowed.";

                ViewBag.Documents = db.Documents.ToList();

                return View("Index");
            }


            string fileName = Path.GetFileName(file.FileName);

            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/files",
                fileName
            );


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            Document doc = new Document();

            doc.FileName = fileName;

            doc.FilePath = "/files/" + fileName;

            doc.UploadedBy = role;

            doc.UploadDate = DateTime.Now;


            db.Documents.Add(doc);

            db.SaveChanges();


            TempData["Message"] =
                "File uploaded successfully";


            Console.WriteLine(DateTime.Now
                + " LOG: File uploaded "
                + fileName);


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFile(int id)
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }


            Document doc = db.Documents.FirstOrDefault(d => d.Id == id);

            if (doc != null)
            {
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    doc.FilePath.TrimStart('/')
                );


                // delete physical file
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }


                // remove database record
                db.Documents.Remove(doc);

                db.SaveChanges();


                Console.WriteLine(DateTime.Now
                    + " LOG: File deleted "
                    + doc.FileName);
            }


            return RedirectToAction("Index");
        }

    }
}
