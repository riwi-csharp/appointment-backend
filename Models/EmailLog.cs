using System;
using System.ComponentModel.DataAnnotations;

namespace appointment_backend.Models
{
    public class EmailLog
    {
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        [Required, EmailAddress]
        public string RecipientEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Body { get; set; }
        
        public int StatusId { get; set; }
        public EmailStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}