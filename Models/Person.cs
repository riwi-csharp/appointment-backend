using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace appointment_backend.Models;

public abstract class Person
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public int DocumentTypeId { get; set; }
    [ValidateNever]
    public DocumentType DocumentType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}