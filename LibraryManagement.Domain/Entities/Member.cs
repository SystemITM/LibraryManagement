namespace LibraryManagement.Domain.Entities;

/*Un miembro puede tener muchos préstamos.
Un préstamo pertenece a un miembro.*/
public class Member : AuditBase
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string DocumentNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}