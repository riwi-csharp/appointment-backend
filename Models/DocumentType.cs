namespace appointment_backend.Models;

public class DocumentType
{
    public int Id { get; set; }
    public string Type { get; set; }
    public List<Doctor> Doctors { get; set; }
    public List<Patient> Patients { get; set; }
}