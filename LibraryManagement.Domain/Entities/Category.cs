namespace LibraryManagement.Domain.Entities;

/*Una categoría puede tener muchos libros.
Un libro pertenece a una categoría.*/

public class Category : AuditBase
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}