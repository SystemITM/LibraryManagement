using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;

    public AuthorsController(IAuthorService authorService, IMapper mapper)
    {
        _authorService = authorService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorResponse>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();

        var response = _mapper.Map<IEnumerable<AuthorResponse>>(authors);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorResponse>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);

        if (author is null)
        {
            return NotFound(new
            {
                message = "El autor no existe."
            });
        }

        var response = _mapper.Map<AuthorResponse>(author);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorResponse>> Create([FromBody] CreateAuthorRequest request)
    {
        try
        {
            var author = _mapper.Map<Author>(request);

            var createdAuthor = await _authorService.CreateAsync(author);

            var response = _mapper.Map<AuthorResponse>(createdAuthor);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
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
    public async Task<ActionResult<AuthorResponse>> Update(int id, [FromBody] UpdateAuthorRequest request)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El Id del autor no es válido."
                });
            }

            var author = _mapper.Map<Author>(request);

            var updatedAuthor = await _authorService.UpdateAsync(id, author);

            var response = _mapper.Map<AuthorResponse>(updatedAuthor);

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
                    message = "El Id del autor no es válido."
                });
            }

            var deleted = await _authorService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "El autor no existe."
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