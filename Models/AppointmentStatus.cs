namespace appointment_backend.Models;

public class AppointmentStatus
{
    public int Id { get; set; }
    public string Status { get; set; }
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
}