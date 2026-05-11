using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Context;
/*Relación 1:N entre Member y Loan
 Un miembro puede tener muchos préstamos.
Un préstamo pertenece a un miembro.
Member 1 ─── N Loan


Relación N:M entre Book y Author
Un libro puede tener muchos autores.
Un autor puede tener muchos libros.
Book N ─── M Author
Como Entity Framework necesita una tabla intermedia, la relación queda así:
Book 1 ─── N BookAuthor N ─── 1 Author
La tabla intermedia será:
BookAuthors
*/
public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Member> Members { get; set; }

    public DbSet<Loan> Loans { get; set; }

    public DbSet<BookAuthor> BookAuthors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureBook(modelBuilder);
        ConfigureAuthor(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigureMember(modelBuilder);
        ConfigureLoan(modelBuilder);
        ConfigureBookAuthor(modelBuilder);
    }

    private static void ConfigureBook(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");

            entity.HasKey(book => book.Id);

            entity.Property(book => book.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(book => book.Isbn)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(book => book.Isbn)
                .IsUnique();

            entity.Property(book => book.PublicationYear)
                .IsRequired();

            entity.Property(book => book.TotalCopies)
                .IsRequired();

            entity.Property(book => book.AvailableCopies)
                .IsRequired();

            entity.Property(book => book.CreatedAt)
                .IsRequired();

            entity.Property(book => book.UpdatedAt)
                .IsRequired(false);
            
            /*
 * Relación 1:N entre Category y Book
Una categoría puede tener muchos libros.
Un libro pertenece a una sola categoría.
Category 1 ─── N Book
 */
            entity.HasOne(book => book.Category)
                .WithMany(category => category.Books)
                .HasForeignKey(book => book.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);//en relaciones como Book -> Loan, Member -> Loan y Category -> Book.
           
/*La idea es evitar que se borren datos importantes por accidente. Por ejemplo:
No debería eliminarse un libro si ya tiene préstamos registrados.
No debería eliminarse un miembro si tiene historial de préstamos.
No debería eliminarse una categoría si tiene libros asociados.*/
        });
    }

    private static void ConfigureAuthor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Authors");

            entity.HasKey(author => author.Id);

            entity.Property(author => author.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(author => author.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(author => author.Biography)
                .HasMaxLength(500);

            entity.Property(author => author.CreatedAt)
                .IsRequired();

            entity.Property(author => author.UpdatedAt)
                .IsRequired(false);

            entity.HasIndex(author => new
            {
                author.FirstName,
                author.LastName
            });
        });
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(category => category.Id);

            entity.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(category => category.Description)
                .HasMaxLength(300);

            entity.Property(category => category.CreatedAt)
                .IsRequired();

            entity.Property(category => category.UpdatedAt)
                .IsRequired(false);

            entity.HasIndex(category => category.Name)
                .IsUnique();
        });
    }

    private static void ConfigureMember(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Members");

            entity.HasKey(member => member.Id);

            entity.Property(member => member.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(member => member.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(member => member.DocumentNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(member => member.DocumentNumber)
                .IsUnique();

            entity.Property(member => member.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(member => member.Email)
                .IsUnique();

            entity.Property(member => member.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(member => member.IsActive)
                .IsRequired();

            entity.Property(member => member.CreatedAt)
                .IsRequired();

            entity.Property(member => member.UpdatedAt)
                .IsRequired(false);
        });
    }

    private static void ConfigureLoan(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("Loans");

            entity.HasKey(loan => loan.Id);

            entity.Property(loan => loan.LoanDate)
                .IsRequired();

            entity.Property(loan => loan.DueDate)
                .IsRequired();

            entity.Property(loan => loan.ReturnDate)
                .IsRequired(false);

            entity.Property(loan => loan.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(loan => loan.CreatedAt)
                .IsRequired();

            entity.Property(loan => loan.UpdatedAt)
                .IsRequired(false);

            entity.HasOne(loan => loan.Book)
                .WithMany(book => book.Loans)
                .HasForeignKey(loan => loan.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(loan => loan.Member)
                .WithMany(member => member.Loans)
                .HasForeignKey(loan => loan.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureBookAuthor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.ToTable("BookAuthors");

            entity.HasKey(bookAuthor => new
            {
                bookAuthor.BookId,
                bookAuthor.AuthorId
            });

            entity.HasOne(bookAuthor => bookAuthor.Book)
                .WithMany(book => book.BookAuthors)
                .HasForeignKey(bookAuthor => bookAuthor.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(bookAuthor => bookAuthor.Author)
                .WithMany(author => author.BookAuthors)
                .HasForeignKey(bookAuthor => bookAuthor.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}