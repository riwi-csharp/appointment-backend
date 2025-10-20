using appointment_backend.Data;
using appointment_backend.Models;
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
        ViewBag.DocumentTypes = await _context.DocumentTypes.ToListAsync();
        ViewBag.Specialities = await _context.Specialities.ToListAsync();
        var doctors = await _context.Doctors.ToListAsync();
        // await GetAll();
        return View(doctors);
    }
    

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
    
    public async Task<bool> ValidarDocumento(string documento)
    {
        var cosulta = await _context.Doctors.FirstOrDefaultAsync(d => d.Document == documento);
        return cosulta == null;  // ture si no existe (puede crear )
    }

    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        // se modifica el try ya que no me permite guardar la informacion 
        // try
        // {
        //     if (await _context.Doctors.AnyAsync(d => d.Document == doctor.Document) != null)
        //     {
        //         ModelState.AddModelError("DocumentAlreadyExists", "Doctor Document already exists");
        //         return RedirectToAction("Index");
        //     }
        //     _context.Doctors.Add(doctor);
        //     await _context.SaveChangesAsync();
        //     TempData["SuccessMessage"] = "Doctor was created";
        // } 
        try
        {
            bool stadoDocumento = await ValidarDocumento(doctor.Document);
            if (!stadoDocumento)
            {
                TempData["ErrorMessage"] = " Ya existe un Documento con este numero. Intente con otro.";
                return RedirectToAction(nameof(Index));
            }
            await _context.AddAsync(doctor);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = " Creado correctamente.";  // para mandar mensajes 
            return RedirectToAction(nameof(Index));

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
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            ViewBag.DocumentTypes = await _context.DocumentTypes.ToListAsync();
            ViewBag.Specialities = await _context.Specialities.ToListAsync();
            var doct = await _context.Doctors.FindAsync(id);
            if (doct == null) return NotFound();

            ViewBag.doct = await _context.Doctors.ToListAsync();

            return View("Edit", doct);
            // if (!ModelState.IsValid) return View(doctor);
            //
            // var getDoctor = await _context.Doctors.FindAsync(doctor.Id);
            // if (getDoctor == null)
            // {
            //     return NotFound();
            // }
            //
            // getDoctor.Name = doctor.Name;
            // getDoctor.Document = doctor.Document;
            // getDoctor.DocumentTypeId = doctor.DocumentTypeId;
            // getDoctor.DocumentType = doctor.DocumentType;
            // getDoctor.Phone = doctor.Phone;
            // getDoctor.Email = doctor.Email;
            // getDoctor.Appointments = doctor.Appointments;
            //
            // _context.Doctors.Update(getDoctor);
            // await _context.SaveChangesAsync();
            // return RedirectToAction("Index"); //Return tu HOME
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // actualizar 
    [HttpGet]
    public async Task<ActionResult> Update(Doctor doctor)
    {
        try
        {
            var existingDoctor = await _context.Doctors.FindAsync(doctor.Id);
            if (existingDoctor == null)
                return NotFound();

            existingDoctor.Name = doctor.Name;
            existingDoctor.Document = doctor.Document;
            existingDoctor.Email = doctor.Email;
            existingDoctor.DocumentTypeId = doctor.DocumentTypeId;
            existingDoctor.SpecialityId = doctor.SpecialityId;
          

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            Console.WriteLine($" Error al actualizar doctor: {e.InnerException?.Message ?? e.Message}");
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
