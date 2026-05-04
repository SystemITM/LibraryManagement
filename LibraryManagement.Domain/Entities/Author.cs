namespace LibraryManagement.Domain.Entities;
/*Un autor puede escribir muchos libros.
Un libro puede tener muchos autores.*/
public class Author : AuditBase
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Biography { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}