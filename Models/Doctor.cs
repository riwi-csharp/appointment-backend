using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace appointment_backend.Models;

public class Doctor : Person
{
    public int SpecialityId { get; set; }
    [ValidateNever]
    public Speciality Speciality { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}