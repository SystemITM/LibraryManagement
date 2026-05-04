namespace LibraryManagement.Domain.Entities;

public class Book : AuditBase
{
    //Relacion Book -> Category
    //Un libro pertenece a una categoría.
    //Book -> BookAuthor -> Author
    //Un libro puede tener varios autores.
    public string Title { get; set; } = string.Empty;

    public string Isbn { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}