namespace CosmosDbPOC.Infrastructure.Exceptions;

public class CosmosDbOperationFailedException(string message) : Exception(message)
{
}
