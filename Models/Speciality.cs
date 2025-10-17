namespace appointment_backend.Models;

public class Speciality
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<DoctorSpeciality>  DoctorSpecialities { get; set; }
}