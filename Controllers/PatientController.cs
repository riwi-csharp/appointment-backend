using appointment_backend.Models;
using MedicalSys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appointment_backend.Controllers;

public class PatientController : Controller
{
    private readonly AppDbContext _context;

    public PatientController(AppDbContext context)
    {
        _context = context;
    }

    //Este me trae todos los pacientes
    public IActionResult Index()
    {
        var patient = _context.Patients.ToList();
        return View(patient);
    }
    
    //Registrar un paciente
    //Si el modelo no es valido, vuelve a mostrar la vista del formulario con los mismos datos que el usuario ingreso.
    public async Task<IActionResult> Register(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient); 
        if (!AgeValid(patient)) return View(patient);
        
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    //Eliminar un paciente
    public async Task<IActionResult> Delete(int id)
    {
        var delete = await _context.Patients.FindAsync(id);
        _context.Patients.Remove(delete);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    //Este busca el paciente por el Id para poder editarlo
    public IActionResult Edit(int id)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null)
        {
            Console.WriteLine("No se a podido encontrar un paciente con ese ID.");
            return NotFound();
        }
        return View(patient);
    }
    
    //Este guarda los cambios al editar un paciente
    public async Task<IActionResult> SaveEdit(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient);
        if (!AgeValid(patient)) return View(patient);
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    //Validar que la esta este dentro del rango del 1 al 100
    public bool AgeValid(Patient patient)
    {
        if (patient.Age < 1 || patient.Age > 100)
        {
            ModelState.AddModelError("Age","La edad ingresada no es valida.");
            return false;
        }
        return true;
    }
}