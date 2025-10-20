using appointment_backend.Data;
using appointment_backend.Models;
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
    
    //METALE ASYNC
    //farag0n -listo ya tiene async
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var patients = await _context.Patients.ToListAsync();
            return View(patients);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocurrio un error inesperado al traer los pacientes\nError: {e}");
            ViewBag.ErrorMessage = "No se pudo cargar la lista de pacientes recargue la pagina";
            return View(Index);
        }
    }

    //Registrar un paciente
    //Si el modelo no es valido, vuelve a mostrar la vista del formulario con los mismos datos que el usuario ingreso.
    [HttpPost]
    public async Task<IActionResult> Register(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient);
        if (!AgeValid(patient)) return View(patient);

        try
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"El paciente no se ha podido guardar el la base de datos\nError{e}");
            ViewBag.ErrorMessage = "No se pudo registrar el paciente, intente de nuevo";
            return View(patient);
        }
        return RedirectToAction("Index");
    }

    //Eliminar un paciente
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var delete = await _context.Patients.FindAsync(id);
        try
        {
            _context.Patients.Remove(delete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"El paciente no se pudo eliminar correctamente{e}");
            ViewBag.ErrorMessage = "Ocurrio un error eliminando el paciente, vuelva a intentarlo mas tarde";
        }
        return RedirectToAction("Index");
    }

    //Este busca el paciente por el Id para poder editarlo
    [HttpPost]
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
    [HttpPost]
    public async Task<IActionResult> SaveEdit(Patient patient)
    {
        if (!ModelState.IsValid) return View(patient);
        if (!AgeValid(patient)) return View(patient);
        try
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"No se pudo guardar la nueva informacion del paciente{e}");
            ViewBag.ErrorMessage = "El paciente no se pudo editar correctamente";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }

    //Validar que la esta este dentro del rango del 1 al 100
    [HttpPost]
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