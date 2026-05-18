using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request;

public class CreateAuthorRequest
{
    [Required(ErrorMessage = "El nombre del autor es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre del autor no puede superar los 100 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido del autor es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El apellido del autor no puede superar los 100 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "La biografía no puede superar los 500 caracteres.")]
    public string? Biography { get; set; }
}