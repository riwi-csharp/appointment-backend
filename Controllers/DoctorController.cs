using appointment_backend.Models;
using MedicalSys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appointment_backend.Controllers;

public class DoctorController : Controller
{

    private readonly AppDbContext  _context;

    public DoctorController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await GetAll();
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var doctors = await _context.Doctors.ToListAsync();
            TempData["SuccessMessage"] = "All doctors were found";
            return RedirectToAction("Index", doctors);
        }
        catch (HttpRequestException e)
        {
            TempData["ErrorMessage"] = $"ERROR: {e.Message}";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"ERROR: {e.Message}";
        }
        return RedirectToAction("Index", null);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        try
        {
            if (_context.Doctors.AnyAsync(d => d.Document == doctor.Document) != null)
            {
                ModelState.AddModelError("DocumentAlreadyExists", "Doctor Document already exists");
                return RedirectToAction("Index");
            }
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Doctor was created";
        }
        catch (HttpRequestException e)
        {
            TempData["ErrorMessage"] = $"ERROR: {e.Message}";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"ERROR: {e.Message}";
        }
        return RedirectToAction("Index");
        
    }
    
    //Jhos Edit / Delete
    public async Task<IActionResult> Edit(Doctor doctor)
    {
        try
        {
            if (!ModelState.IsValid) return View(doctor);
        
            var getDoctor = await _context.Doctors.FindAsync(doctor.Id);
            if (getDoctor == null)
            {
                return NotFound();
            }
        
            getDoctor.Name = doctor.Name;
            getDoctor.Document = doctor.Document;
            getDoctor.DocumentTypeId = doctor.DocumentTypeId;
            getDoctor.DocumentType = doctor.DocumentType;
            getDoctor.Phone = doctor.Phone;
            getDoctor.Email = doctor.Email;
            getDoctor.DoctorSpecialities = doctor.DoctorSpecialities;
            getDoctor.Appointments = doctor.Appointments;
        
            _context.Doctors.Update(getDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); //Return tu HOME
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(doctor => doctor.Id == id);
            if (doctor == null)
            {
                Console.WriteLine("Doctor not found");
            }
            else
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                Console.WriteLine("doctor successfully eliminated");
            }
            return RedirectToAction("Index"); //Return tu HOME
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
