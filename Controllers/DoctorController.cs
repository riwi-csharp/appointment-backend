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
}