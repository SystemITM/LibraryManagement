namespace LibraryManagement.Domain.Entities;

//Book 1:N BookAuthor N:1 Author
/*Un libro puede tener varios autores.
Un autor puede escribir varios libros.*/
public class BookAuthor
{
    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;
}