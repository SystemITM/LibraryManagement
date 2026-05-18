namespace LibraryManagement.API.DTOs.Response;

public class BookResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Isbn { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public List<BookAuthorResponse> Authors { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}