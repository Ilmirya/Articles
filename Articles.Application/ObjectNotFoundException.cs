namespace Articles.Application;

public class ObjectNotFoundException(string objectName) : Exception($"{objectName} not found");