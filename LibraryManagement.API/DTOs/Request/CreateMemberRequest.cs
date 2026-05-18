using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request;

public class CreateMemberRequest
{
    [Required(ErrorMessage = "El nombre del miembro es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre del miembro no puede superar los 100 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido del miembro es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El apellido del miembro no puede superar los 100 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El número de documento no puede superar los 30 caracteres.")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [MaxLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres.")]
    public string PhoneNumber { get; set; } = string.Empty;
}