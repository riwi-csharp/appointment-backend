using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using appointment_backend.Models;
using appointment_backend.Models.Email;
using appointment_backend.Services;

namespace appointment_backend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EmailService _emailService;

    public HomeController(ILogger<HomeController> logger, EmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> TestEmail()
    {
        var emailBody = $@"
            <html>
                <body>
                    <h1>HOLA JUAN</h1>
                    <p>LLAMADO DE EMERGENCIA</p>
                </body>
            </html>
        ";

        var response = await _emailService.SendEmailAsync(new EmailMessage
        {
            To = "juanda2026@gmail.com",
            Subject = "PROBANDO",
            Body = emailBody,
            IsHtml = true
        });

        if (response)
        {
            ViewBag.EmailMessage = "TODO SALIO PERFECTO LOCO.";
            return View(nameof(Index));
        }
        
        
        ViewBag.EmailMessage = null;
        return View(nameof(Index));
    }
}