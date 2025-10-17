namespace appointment_backend.Models;

public class Doctor : Person
{

    public int SpecialityId { get; set; }
    public Speciality Speciality { get; set; }


    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}