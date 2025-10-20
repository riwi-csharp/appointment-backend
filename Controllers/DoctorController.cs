using appointment_backend.Data;
using appointment_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        try
        {
            var doctors = await _context.Doctors.Include(d=> d.DocumentType).ToListAsync();

            // Puedes usar ViewBag si quieres mostrar un mensaje en la vista
            ViewBag.SuccessMessage = "Lista de doctores cargada correctamente";

            // Devuelves la vista junto con la lista como modelo
            return View(doctors);
        }
        catch (HttpRequestException e)
        {
            ViewBag.ErrorMessage = $"Error de conexión: {e.Message}";
            return View(new List<Doctor>()); // Devuelve vista vacía
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = $"Error inesperado: {e.Message}";
            return View(new List<Doctor>());
        }
    }


    // CODIGO DE JAVIER
    // [HttpGet]
    // public async Task<IActionResult> Index()
    // {
    //     await GetAll();
    //     return View();
    // }
    //
    // [HttpGet]
    // public async Task<IActionResult> GetAll()
    // {
    //     try
    //     {
    //         var doctors = await _context.Doctors.ToListAsync();
    //         TempData["SuccessMessage"] = "All doctors were found";
    //         return RedirectToAction("Index", doctors);
    //     }
    //     catch (HttpRequestException e)
    //     {
    //         TempData["ErrorMessage"] = $"ERROR: {e.Message}";
    //     }
    //     catch (Exception e)
    //     {
    //         TempData["ErrorMessage"] = $"ERROR: {e.Message}";
    //     }
    //     return RedirectToAction("Index", null);
    // }
    
    public IActionResult Register()
    {
        ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Type");
        ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Doctor doctor)
    {
        // Verifica si el modelo cumple con las validaciones de DataAnnotations
        if (!ModelState.IsValid)
        {
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Type");
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
            return View(doctor);
        }

        try
        {
            // ✅ Verifica si ya existe un doctor con el mismo documento
            bool exists = await _context.Doctors.AnyAsync(d => d.Document == doctor.Document);
            if (exists)
            {
                ModelState.AddModelError("Document", "El número de documento ya está registrado.");
                ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Type");
                ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
                return View(doctor); // Mantiene los datos y muestra el mensaje
            }

            // ✅ Guarda el nuevo doctor
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "El doctor fue registrado correctamente.";
            return RedirectToAction("Index");
        }
        catch (HttpRequestException e)
        {
            TempData["ErrorMessage"] = $"Error de conexión: {e.Message}";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error inesperado: {e.Message}";
            return RedirectToAction("Index");
        }
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
