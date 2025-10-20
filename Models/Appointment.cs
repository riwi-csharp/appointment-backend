using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace appointment_backend.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        
        public string ConsultingRoom { get; set; }
        public string AppointmentCode { get; set; } = $"APT-{DateTime.Now:yyyyMMddHHmmss}";

        [Required]
        public int? DoctorId { get; set; }    
        [ValidateNever]
        public Doctor? Doctor { get; set; }   

        [Required]
        public int? PatientId { get; set; }   
        [ValidateNever]
        public Patient? Patient { get; set; } 

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }

        public int StatusId { get; set; }
        [ValidateNever]
        public AppointmentStatus Status { get; set; }

        public string? Notes { get; set; }
    }
}