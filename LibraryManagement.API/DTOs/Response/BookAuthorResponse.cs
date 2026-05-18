namespace LibraryManagement.API.DTOs.Response;

public class BookAuthorResponse
{
    public int AuthorId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
}