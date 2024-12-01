
namespace DO;


[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
[Serializable]
public class DalAlreadyExistsException : Exception// האובייקט כבר קיים
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
public class DalDeletionImpossible : Exception// המחיקה בלתי אפשרית"
{
    public DalDeletionImpossible(string? message) : base(message) { }
}