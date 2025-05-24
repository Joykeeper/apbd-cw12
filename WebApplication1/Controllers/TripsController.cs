using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services;

namespace WebApplication1.Controllers;
    

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _dbService.GetTripsAsync(page, pageSize);
            return Ok(result);
        } catch (NoTripsOnPageException e)
        {
            return BadRequest(e.Message);
        } catch (NoTripsException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientToTripDto dto)
    {
        try
        {
            await _dbService.AssignClientToTripAsync(idTrip, dto);
            return Ok(new { message = "Client assigned to trip successfully!" });
        }
        catch (ClientAlreadyOnTripException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (TripNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }

}