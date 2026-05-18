using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public BooksController(IBookService bookService, IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();

        var response = _mapper.Map<IEnumerable<BookResponse>>(books);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponse>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null)
        {
            return NotFound(new
            {
                message = "El libro no existe."
            });
        }

        var response = _mapper.Map<BookResponse>(book);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create([FromBody] CreateBookRequest request)
    {
        try
        {
            var book = _mapper.Map<Book>(request);

            var createdBook = await _bookService.CreateAsync(book, request.AuthorIds);

            var response = _mapper.Map<BookResponse>(createdBook);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponse>> Update(int id, [FromBody] UpdateBookRequest request)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El Id del libro no es válido."
                });
            }

            var book = _mapper.Map<Book>(request);

            var updatedBook = await _bookService.UpdateAsync(id, book, request.AuthorIds);

            var response = _mapper.Map<BookResponse>(updatedBook);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El Id del libro no es válido."
                });
            }

            var deleted = await _bookService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "El libro no existe."
                });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}