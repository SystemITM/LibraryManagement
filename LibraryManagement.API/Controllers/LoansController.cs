using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly IMapper _mapper;

    public LoansController(ILoanService loanService, IMapper mapper)
    {
        _loanService = loanService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanResponse>>> GetAll()
    {
        var loans = await _loanService.GetAllAsync();

        var response = _mapper.Map<IEnumerable<LoanResponse>>(loans);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanResponse>> GetById(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);

        if (loan is null)
        {
            return NotFound(new
            {
                message = "El préstamo no existe."
            });
        }

        var response = _mapper.Map<LoanResponse>(loan);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoanResponse>> Create([FromBody] CreateLoanRequest request)
    {
        try
        {
            var createdLoan = await _loanService.CreateLoanAsync(
                request.BookId,
                request.MemberId,
                request.DueDate);

            var response = _mapper.Map<LoanResponse>(createdLoan);

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

    [HttpPut("{id:int}/return")]
    public async Task<ActionResult<LoanResponse>> ReturnLoan(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El Id del préstamo no es válido."
                });
            }

            var returnedLoan = await _loanService.ReturnLoanAsync(id);

            var response = _mapper.Map<LoanResponse>(returnedLoan);

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
                    message = "El Id del préstamo no es válido."
                });
            }

            var deleted = await _loanService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "El préstamo no existe."
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