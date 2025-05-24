namespace WebApplication1.DTOs;


public class TripCountryDto
{
    public string Name { get; set; }
}


public class TripClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}


public class TripDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<TripCountryDto> Countries { get; set; }
    public List<TripClientDto> Clients { get; set; }
}


public class TripResponseDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDto> Trips { get; set; }
}

public class AssignClientToTripDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telephone { get; set; } = null!;
    public string Pesel { get; set; } = null!;
    public DateTime? PaymentDate { get; set; }
}