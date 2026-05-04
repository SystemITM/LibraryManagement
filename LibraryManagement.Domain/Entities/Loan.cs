using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

/*Un préstamo pertenece a un libro.
Un préstamo pertenece a un miembro.
Un libro puede aparecer en muchos préstamos.
Un miembro puede tener muchos préstamos.*/

public class Loan : AuditBase
{
    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public int MemberId { get; set; }

    public Member Member { get; set; } = null!;

    public DateTime LoanDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public LoanStatus Status { get; set; } = LoanStatus.Active;
}