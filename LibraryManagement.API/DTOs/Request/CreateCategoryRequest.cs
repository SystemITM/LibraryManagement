using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre de la categoría no puede superar los 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
    public string? Description { get; set; }
}