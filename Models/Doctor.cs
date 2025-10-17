namespace appointment_backend.Models;

public class Doctor : Person
{

    public List<DoctorSpeciality>  DoctorSpecialities { get; set; }


    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}