using DO;

namespace BO;

internal class Exceptions
{
    private string v;
    private DalAlreadyExistsException ex;

    public Exceptions(string v, DalAlreadyExistsException ex)
    {
        this.v = v;
        this.ex = ex;
    }
}
