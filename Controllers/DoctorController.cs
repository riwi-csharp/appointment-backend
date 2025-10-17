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
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _context.Doctors.ToListAsync();
        return RedirectToAction("Index", doctors);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}