using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IMapper _mapper;

    public MembersController(IMemberService memberService, IMapper mapper)
    {
        _memberService = memberService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAll()
    {
        var members = await _memberService.GetAllAsync();

        var response = _mapper.Map<IEnumerable<MemberResponse>>(members);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberResponse>> GetById(int id)
    {
        var member = await _memberService.GetByIdAsync(id);

        if (member is null)
        {
            return NotFound(new
            {
                message = "El miembro no existe."
            });
        }

        var response = _mapper.Map<MemberResponse>(member);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponse>> Create([FromBody] CreateMemberRequest request)
    {
        try
        {
            var member = _mapper.Map<Member>(request);

            var createdMember = await _memberService.CreateAsync(member);

            var response = _mapper.Map<MemberResponse>(createdMember);

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
    public async Task<ActionResult<MemberResponse>> Update(int id, [FromBody] UpdateMemberRequest request)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El Id del miembro no es válido."
                });
            }

            var member = _mapper.Map<Member>(request);

            var updatedMember = await _memberService.UpdateAsync(id, member);

            var response = _mapper.Map<MemberResponse>(updatedMember);

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
                    message = "El Id del miembro no es válido."
                });
            }

            var deleted = await _memberService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "El miembro no existe."
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