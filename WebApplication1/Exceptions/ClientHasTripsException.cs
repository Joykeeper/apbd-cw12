namespace WebApplication1.Exceptions;

public class ClientHasTripsException() : Exception("Client cannot be deleted because they are assigned to at least one trip.");