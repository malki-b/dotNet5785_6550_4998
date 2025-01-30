
namespace DO;
/// <summary>
/// When trying to update an object with an ID number that does not exist in the object list
/// </summary>

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
/// <summary>
/// The object already exists.
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
/// <summary>
/// Error creating assignment
/// </summary>
public class ErrorCreatingAssignment : Exception
{
    public ErrorCreatingAssignment(string? message) : base(message) { 
    }
}

public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message)
    {
    }
}

[Serializable]
public class DalNotFoundException : Exception
{
    public DalNotFoundException()
    {
    }

    public DalNotFoundException(string? message) : base(message)
    {
    }

    public DalNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}