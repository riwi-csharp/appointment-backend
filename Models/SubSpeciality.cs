namespace appointment_backend.Models;

public class SubSpeciality
{
    public int Id { get; set; }
    public string SubSpecialityName { get; set; }
    public List<DoctorSpeciality> DoctorSpecialities { get; set; }
}