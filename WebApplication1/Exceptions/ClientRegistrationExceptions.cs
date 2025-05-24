namespace WebApplication1.Exceptions;

public class ClientAlreadyOnTripException() : Exception("Client is already assigned to this trip.");

public class TripNotFoundException() : Exception("Trip not found or already started.");