using appointment_backend.Data;
using appointment_backend.Models;
using appointment_backend.Models.Email;
using appointment_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appointment_backend.Controllers;

public class AppointmentController : Controller
{
    // To send emails
    private readonly EmailService _emailService;
    private readonly AppDbContext _context;

    public AppointmentController(AppDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // Get the appointments in the index so we can list them
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Status)
            .ToListAsync();

        return View(appointments);
    }

    // here we get an appointment so we can show all its details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (appointment == null) return NotFound();

        return View(appointment);
    }

    // Now we render the create view which would contain the next information
    public IActionResult Create()
    {
        ViewData["Patients"] = _context.Patients.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Statuses"] = _context.AppointmentStatuses.ToList();
        return View();
    }

    // with this we would use the functionality of create I meant (POST)
    [HttpPost]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Patients"] = _context.Patients.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Statuses"] = _context.AppointmentStatuses.ToList();

        return View(appointment);
    }

    // in order for we can edit, here we render the edit view with that information that the database already has
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return NotFound();

        ViewData["Patients"] = _context.Patients.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Statuses"] = _context.AppointmentStatuses.ToList();

        return View(appointment);
    }

    // and here we use the functionality of edit an appointment getting it for the ID when I select the specific appointment for being edited
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Appointment appointment)
    {
        if (id != appointment.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Appointments.Any(e => e.Id == appointment.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["Patients"] = _context.Patients.ToList();
        ViewData["Doctors"] = _context.Doctors.ToList();
        ViewData["Statuses"] = _context.AppointmentStatuses.ToList();

        return View(appointment);
    }

    // Here we are rendering the delete action
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (appointment == null) return NotFound();

        return View(appointment);
    }

    // using the delete functionality to apply it and could delete an appointment
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> SendEmail(int id)
    {
        
        var appointmentEmail = await _context.Patients.FindAsync(id);
        
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        var emailBody = $@"
        <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{{{ font-family: Arial, sans-serif; line-height: 1.1; color: #333; }}}}
                    .container {{{{ max-width: 600px; margin: 0 auto; padding: 20px; }}}}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Saint Vincent's Hospital<h1/>
                    <h3>Notification of C# .NET | Hopper <h3/>
                    <p>Hola {appointmentEmail.Name}</p>
                    <p>A continuación podrás ver el detalle de tu cita médica en el H. San Vicente.</p>
                    <p>Estado: {appointment.Status.Status}</p>

                    <table style=""border: 1px solid black; border-collapse: collapse;"">
                        <thead>
                            <tr>
                                <th style=""border: 1px solid black;"">Nombre médico</th>
                                <th style=""border: 1px solid black;"">Fecha</th>
                                <th style=""border: 1px solid black;"">Precio</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style=""border: 1px solid black;"">{appointment.Doctor.Name}</td>
                                <td style=""border: 1px solid black;"">{appointment.Date}</td>
                                <td style=""border: 1px solid black;"">$259.900</td>
                            </tr>
                        </tbody>
                    </table>

                    <p>Gracias por confiar en nuestros servicios.</p>

                    <hr>

                    <p>Este mensaje fue enviado automáticamente, favor no responderlo.</p>
                </div>
            </body>
            </html>
        ";

        try
        {
            var res = await _emailService.SendEmailAsync(new EmailMessage
                {
                    To = appointmentEmail.Email,
                    Subject = "Notificación de su cita.",
                    Body = emailBody,
                    IsHtml = true
                }
            );

            if (res)
            {
                Console.WriteLine("TODO BIEN");
            }
            else
            {
                Console.WriteLine("TODO MAL");
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine("It has presented an error: " + ex);
            return View("Index");
        }
    }
}
