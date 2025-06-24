namespace Articles.Application.Exceptions;

public class ObjectNotFoundException(string objectName) : Exception($"{objectName} not found");