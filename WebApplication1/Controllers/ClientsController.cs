using Microsoft.AspNetCore.Mvc;
using WebApplication1.Exceptions;
using WebApplication1.Services;

namespace WebApplication1.Controllers;
    
[Route("api/[controller]")]
[ApiController]
public class ClientsController(IDbService dbService) : ControllerBase
{
    private readonly IDbService _dbService = dbService;

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _dbService.DeleteClientAsync(idClient);
            return Ok("Client deleted");
        }
        catch (ClientNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (ClientHasTripsException e)
        {
            return Conflict(new { message = e.Message });
        }
    }
}
    

