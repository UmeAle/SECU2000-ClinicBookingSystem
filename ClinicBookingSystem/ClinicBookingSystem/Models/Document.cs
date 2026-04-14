/*
 * FILE             : Document.cs
 * PROJECT          : Clinic Booking System
 * PROGRAMMERS      : Eumee Garcia
 * FIRST VERSION    : 2026-04-12
 * DESCRIPTION      : Stores uploaded file information for the system.
 */

namespace ClinicBookingSystem.Models
{
    public class Document
    {
        // Unique identifier for document
        public int Id { get; set; }

        // Name of uploaded file
        public string FileName { get; set; }

        // Relative file storage path
        public string FilePath { get; set; }

        // Role or user who uploaded file
        public string UploadedBy { get; set; }

        // Date and time file was uploaded
        public DateTime UploadDate { get; set; }
    }
}
