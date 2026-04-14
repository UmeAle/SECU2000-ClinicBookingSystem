/*
 * FILE			         : Document.cs
 * PROJECT		         : Clinic Booking System
 * PROGRAMMERS	         : Eumee Garcia
 * FIRST VERSION         : 2026-04-12
 * DESCRIPTION	         : The purpose of this is to...
 */

namespace ClinicBookingSystem.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string UploadedBy { get; set; }   

        public DateTime UploadDate { get; set; }
    }
}
