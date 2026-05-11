using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        if (await context.Categories.AnyAsync())
        {
            return;
        }

        var categories = new List<Category>
        {
            new()
            {
                Name = "Programación",
                Description = "Libros relacionados con desarrollo de software y buenas prácticas.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                Name = "Bases de Datos",
                Description = "Libros sobre modelado, diseño y administración de bases de datos.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                Name = "Ingeniería de Software",
                Description = "Libros sobre arquitectura, análisis, diseño y gestión de proyectos de software.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                Name = "Ciberseguridad",
                Description = "Libros sobre seguridad informática, protección de sistemas y gestión del riesgo.",
                CreatedAt = DateTime.Now
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        var authors = new List<Author>
        {
            new()
            {
                FirstName = "Robert",
                LastName = "Martin",
                Biography = "Autor reconocido por sus aportes a buenas prácticas de programación y arquitectura limpia.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                FirstName = "Martin",
                LastName = "Fowler",
                Biography = "Autor especializado en arquitectura de software, refactorización y patrones empresariales.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                FirstName = "Abraham",
                LastName = "Silberschatz",
                Biography = "Autor reconocido en áreas de sistemas operativos y bases de datos.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                FirstName = "Bruce",
                LastName = "Schneier",
                Biography = "Especialista y autor reconocido en seguridad informática y criptografía.",
                CreatedAt = DateTime.Now
            }
        };

        await context.Authors.AddRangeAsync(authors);
        await context.SaveChangesAsync();

        var members = new List<Member>
        {
            new()
            {
                FirstName = "Laura",
                LastName = "Gómez",
                DocumentNumber = "1001001001",
                Email = "laura.gomez@email.com",
                PhoneNumber = "3001234567",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new()
            {
                FirstName = "Carlos",
                LastName = "Restrepo",
                DocumentNumber = "1001001002",
                Email = "carlos.restrepo@email.com",
                PhoneNumber = "3019876543",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new()
            {
                FirstName = "Mariana",
                LastName = "Torres",
                DocumentNumber = "1001001003",
                Email = "mariana.torres@email.com",
                PhoneNumber = "3025558899",
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        };

        await context.Members.AddRangeAsync(members);
        await context.SaveChangesAsync();

        var books = new List<Book>
        {
            new()
            {
                Title = "Clean Code",
                Isbn = "9780132350884",
                PublicationYear = 2008,
                TotalCopies = 5,
                AvailableCopies = 4,
                CategoryId = categories[0].Id,
                CreatedAt = DateTime.Now,
                BookAuthors = new List<BookAuthor>
                {
                    new()
                    {
                        AuthorId = authors[0].Id
                    }
                }
            },
            new()
            {
                Title = "Refactoring",
                Isbn = "9780201485677",
                PublicationYear = 1999,
                TotalCopies = 4,
                AvailableCopies = 4,
                CategoryId = categories[2].Id,
                CreatedAt = DateTime.Now,
                BookAuthors = new List<BookAuthor>
                {
                    new()
                    {
                        AuthorId = authors[1].Id
                    }
                }
            },
            new()
            {
                Title = "Database System Concepts",
                Isbn = "9780078022159",
                PublicationYear = 2010,
                TotalCopies = 3,
                AvailableCopies = 2,
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.Now,
                BookAuthors = new List<BookAuthor>
                {
                    new()
                    {
                        AuthorId = authors[2].Id
                    }
                }
            },
            new()
            {
                Title = "Applied Cryptography",
                Isbn = "9781119096726",
                PublicationYear = 2015,
                TotalCopies = 2,
                AvailableCopies = 2,
                CategoryId = categories[3].Id,
                CreatedAt = DateTime.Now,
                BookAuthors = new List<BookAuthor>
                {
                    new()
                    {
                        AuthorId = authors[3].Id
                    }
                }
            }
        };

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();

        var loans = new List<Loan>
        {
            new()
            {
                BookId = books[0].Id,
                MemberId = members[0].Id,
                LoanDate = DateTime.Now.AddDays(-3),
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = null,
                Status = LoanStatus.Active,
                CreatedAt = DateTime.Now
            },
            new()
            {
                BookId = books[2].Id,
                MemberId = members[1].Id,
                LoanDate = DateTime.Now.AddDays(-10),
                DueDate = DateTime.Now.AddDays(-2),
                ReturnDate = null,
                Status = LoanStatus.Overdue,
                CreatedAt = DateTime.Now
            },
            new()
            {
                BookId = books[1].Id,
                MemberId = members[2].Id,
                LoanDate = DateTime.Now.AddDays(-15),
                DueDate = DateTime.Now.AddDays(-5),
                ReturnDate = DateTime.Now.AddDays(-6),
                Status = LoanStatus.Returned,
                CreatedAt = DateTime.Now
            }
        };

        await context.Loans.AddRangeAsync(loans);
        await context.SaveChangesAsync();
    }
}