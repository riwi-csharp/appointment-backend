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
    public async Task<IActionResult> Index()
    {
        var patient = await _context.Patients.ToListAsync();
        return View(patient);
    }

    //Registrar un paciente
    //Si el modelo no es valido, vuelve a mostrar la vista del formulario con los mismos datos que el usuario ingreso.
    public async Task<IActionResult> Register(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient);
        if (!AgeValid(patient)) return View(patient);
        if (!PhoneValid(patient)) return View(patient);
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
        if (!PhoneValid(patient)) return View(patient);
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    //Validar que la esta este dentro del rango del 1 al 100
    //Valida de que si la edad es 0 o null deba ingresar una edad
    public bool AgeValid(Patient patient)
    {
        if (patient.Age == 0 || patient.Age == null)
        {
            ModelState.AddModelError("Age", "Debes Ingresar una edad.");
            return false;
        }
        if (patient.Age < 1 || patient.Age > 100)
        {
            ModelState.AddModelError("Age", "La edad ingresada no es valida.");
            return false;
        }
        return true;
    }

    //Validar de que el numero de telefono tenga 10 digitos exactos y que no ingrese con letras 
    //Muestra mensaje si intenta registrar un numero en null,vacio o en espacio en blanco
    public bool PhoneValid(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.Phone))
        {
            ModelState.AddModelError("Phone", "Debes ingresar un numero de telefono.");
        }
        if (patient.Phone.Length != 10 || !patient.Phone.All(char.IsDigit))
        {
            ModelState.AddModelError("Phone", "El numero de telefono debe de tener exactamente 10 digitos.");
            return false;
        }
        return true;
    }
}