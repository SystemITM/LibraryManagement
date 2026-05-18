using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        var response = _mapper.Map<IEnumerable<CategoryResponse>>(categories);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound(new
            {
                message = "La categoría no existe."
            });
        }

        var response = _mapper.Map<CategoryResponse>(category);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CreateCategoryRequest request)
    {
        try
        {
            var category = _mapper.Map<Category>(request);

            var createdCategory = await _categoryService.CreateAsync(category);

            var response = _mapper.Map<CategoryResponse>(createdCategory);

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
    public async Task<ActionResult<CategoryResponse>> Update(int id, [FromBody] UpdateCategoryRequest request)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El Id de la categoría no es válido."
                });
            }

            var category = _mapper.Map<Category>(request);

            var updatedCategory = await _categoryService.UpdateAsync(id, category);

            var response = _mapper.Map<CategoryResponse>(updatedCategory);

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
                    message = "El Id de la categoría no es válido."
                });
            }

            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "La categoría no existe."
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