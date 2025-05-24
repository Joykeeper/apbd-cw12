using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class DbService : IDbService
{
    private readonly MyDbContext _context;

    public DbService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<TripResponseDto> GetTripsAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom);

        var totalTrips = await query.CountAsync();

        if (totalTrips == 0)
            throw new NoTripsException();
        
        var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new TripCountryDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new TripClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();
    
        if (trips.Count == 0)
            throw new NoTripsOnPageException();
        
        return new TripResponseDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = trips
        };
    }
    
    public async Task DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients.FindAsync(idClient);
        if (client == null)
            throw new ClientNotFoundException();
        
        var hasTrips = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
        if (hasTrips)
            throw new ClientHasTripsException();
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
    
    public async Task AssignClientToTripAsync(int idTrip, AssignClientToTripDto dto)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);

        if (existingClient != null)
        {
            var alreadyOnTrip = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);

            if (alreadyOnTrip)
                throw new ClientAlreadyOnTripException();
        }


        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);

        if (trip == null || trip.DateFrom <= DateTime.Now)
            throw new TripNotFoundException();
        
        if (existingClient == null)
        {
            existingClient = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };

            _context.Clients.Add(existingClient);
            await _context.SaveChangesAsync();
        }
        
        var clientTrip = new ClientTrip
        {
            IdClient = existingClient.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }
    
}

