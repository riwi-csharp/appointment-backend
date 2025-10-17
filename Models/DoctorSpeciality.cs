namespace appointment_backend.Models;

public class DoctorSpeciality
{
    public int DoctorId { get; set; }
    public int SpecialityId { get; set; }
    public int SubSpecialityId { get; set; }
    public Speciality Speciality { get; set; }
    public SubSpeciality SubSpeciality { get; set; }
    public Doctor Doctor { get; set; }
}