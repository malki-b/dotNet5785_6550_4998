
namespace DO
{
    [Serializable]
    internal class DalDoNotSuccseedDelete : Exception
    {
        public DalDoNotSuccseedDelete()
        {
        }

        public DalDoNotSuccseedDelete(string? message) : base(message)
        {
        }

        public DalDoNotSuccseedDelete(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}