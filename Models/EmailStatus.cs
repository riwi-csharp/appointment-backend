namespace appointment_backend.Models;

public class EmailStatus
{
    public int Id { get; set; }
    public string Status { get; set; }
    public List<EmailLog> Emails { get; set; } = new List<EmailLog>();
}