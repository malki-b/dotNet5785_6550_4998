using DO;

namespace BO;


[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
   

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}


[Serializable]
internal class BlCannotDeleteException : Exception
{
    public BlCannotDeleteException()
    {
    }

    public BlCannotDeleteException(string? message) : base(message)
    {
    }

    public BlCannotDeleteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }


}

[Serializable]
public class BlInvalidException : Exception
{
    public BlInvalidException(string? message) : base(message) { }
    public BlInvalidException(string message, Exception innerException)
                : base(message, innerException) { }
}