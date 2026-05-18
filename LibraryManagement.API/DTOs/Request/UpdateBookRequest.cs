using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request;

public class UpdateBookRequest
{
    [Required(ErrorMessage = "El título del libro es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El título del libro no puede superar los 150 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN del libro es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El ISBN no puede superar los 20 caracteres.")]
    public string Isbn { get; set; } = string.Empty;

    [Range(1000, 9999, ErrorMessage = "El año de publicación no es válido.")]
    public int PublicationYear { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El total de copias debe ser mayor a cero.")]
    public int TotalCopies { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Las copias disponibles no pueden ser negativas.")]
    public int AvailableCopies { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar al menos un autor.")]
    public List<int> AuthorIds { get; set; } = new();
}