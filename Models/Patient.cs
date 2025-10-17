namespace appointment_backend.Models;

public class Patient : Person
{
    public int Age { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}