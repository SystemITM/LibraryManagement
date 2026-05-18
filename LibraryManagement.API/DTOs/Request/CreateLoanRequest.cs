using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request;

public class CreateLoanRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido.")]
    public int BookId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un miembro válido.")]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "La fecha esperada de devolución es obligatoria.")]
    public DateTime DueDate { get; set; }
}