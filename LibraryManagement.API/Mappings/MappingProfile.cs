using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Authors
        CreateMap<CreateAuthorRequest, Author>();

        CreateMap<UpdateAuthorRequest, Author>();

        CreateMap<Author, AuthorResponse>()
            .ForMember(
                destination => destination.FullName,
                option => option.MapFrom(source => $"{source.FirstName} {source.LastName}".Trim()))
            .ForMember(
                destination => destination.BookCount,
                option => option.MapFrom(source => source.BookAuthors.Count));

        // Categories
        CreateMap<CreateCategoryRequest, Category>();

        CreateMap<UpdateCategoryRequest, Category>();

        CreateMap<Category, CategoryResponse>()
            .ForMember(
                destination => destination.BookCount,
                option => option.MapFrom(source => source.Books.Count));

        // Members
        CreateMap<CreateMemberRequest, Member>()
            .ForMember(
                destination => destination.IsActive,
                option => option.MapFrom(_ => true));

        CreateMap<UpdateMemberRequest, Member>();

        CreateMap<Member, MemberResponse>()
            .ForMember(
                destination => destination.FullName,
                option => option.MapFrom(source => $"{source.FirstName} {source.LastName}".Trim()))
            .ForMember(
                destination => destination.LoanCount,
                option => option.MapFrom(source => source.Loans.Count));

        // Books
        CreateMap<CreateBookRequest, Book>()
            .ForMember(
                destination => destination.AvailableCopies,
                option => option.MapFrom(source => source.TotalCopies))
            .ForMember(
                destination => destination.BookAuthors,
                option => option.Ignore())
            .ForMember(
                destination => destination.Loans,
                option => option.Ignore())
            .ForMember(
                destination => destination.Category,
                option => option.Ignore());

        CreateMap<UpdateBookRequest, Book>()
            .ForMember(
                destination => destination.BookAuthors,
                option => option.Ignore())
            .ForMember(
                destination => destination.Loans,
                option => option.Ignore())
            .ForMember(
                destination => destination.Category,
                option => option.Ignore());

        CreateMap<BookAuthor, BookAuthorResponse>()
            .ForMember(
                destination => destination.AuthorId,
                option => option.MapFrom(source => source.AuthorId))
            .ForMember(
                destination => destination.FirstName,
                option => option.MapFrom(source => source.Author.FirstName))
            .ForMember(
                destination => destination.LastName,
                option => option.MapFrom(source => source.Author.LastName))
            .ForMember(
                destination => destination.FullName,
                option => option.MapFrom(source => $"{source.Author.FirstName} {source.Author.LastName}".Trim()));

        CreateMap<Book, BookResponse>()
            .ForMember(
                destination => destination.CategoryName,
                option => option.MapFrom(source => source.Category.Name))
            .ForMember(
                destination => destination.Authors,
                option => option.MapFrom(source => source.BookAuthors));

        // Loans
        CreateMap<CreateLoanRequest, Loan>()
            .ForMember(
                destination => destination.Book,
                option => option.Ignore())
            .ForMember(
                destination => destination.Member,
                option => option.Ignore());

        CreateMap<Loan, LoanResponse>()
            .ForMember(
                destination => destination.BookTitle,
                option => option.MapFrom(source => source.Book.Title))
            .ForMember(
                destination => destination.MemberFullName,
                option => option.MapFrom(source => $"{source.Member.FirstName} {source.Member.LastName}".Trim()))
            .ForMember(
                destination => destination.Status,
                option => option.MapFrom(source => source.Status.ToString()));
    }
}