using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IDbService
{
    Task<TripResponseDto> GetTripsAsync(int page, int pageSize);
    
    Task DeleteClientAsync(int idClient);
    
    Task AssignClientToTripAsync(int idTrip, AssignClientToTripDto dto);
}