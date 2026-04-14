/*
 * FILE             : UploadController.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Handles secure file upload, storage,
 *                    and deletion for admin users.
 */

using Microsoft.AspNetCore.Mvc;
using System.IO;
using ClinicBookingSystem.Models;
using ClinicBookingSystem.Database;

namespace ClinicBookingSystem.Controllers
{
    public class UploadController : Controller
    {
        // Database context used to store file metadata
        private readonly ApplicationDbContext db;

        /*
         * FUNCTION     : UploadController (Constructor)
         * DESCRIPTION  : Initializes controller with database context.
         */
        public UploadController(ApplicationDbContext context)
        {
            db = context;
        }

        /*
       * FUNCTION     : Index
       * DESCRIPTION  : Displays uploaded documents list.
       *                Only accessible to Admin users.
       */
        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Role = role;

            // Retrieve uploaded files sorted by newest
            List<Document> docs = db.Documents
                .OrderByDescending(d => d.UploadDate)
                .ToList();

            ViewBag.Documents = docs;

            return View();
        }

        /*
         * FUNCTION     : UploadFile
         * DESCRIPTION  : Uploads file to server and stores metadata
         *                in database.
         */
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

            //save file to server
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

            //save file metadata to database
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

        /*
        * FUNCTION     : DeleteFile
        * DESCRIPTION  : Deletes file from server and removes
        *                record from database.
        */
        [HttpPost]
        public IActionResult DeleteFile(int id)
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // Find document record
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
