using appointment_backend.Data;
using appointment_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appointment_backend.Controllers;

public class AppointmentController : Controller
{
    private readonly AppDbContext _context;

    public AppointmentController(AppDbContext context)
    {
        _context = context;
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
}
