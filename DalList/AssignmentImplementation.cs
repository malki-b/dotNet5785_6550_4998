

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class AssignmentImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        int idCall = Config.nextAssignmentId;
        Assignment copy = item;
        copy.Id = idCall;
        DataSource.Assignments.Add(copy);   
       return copy.Id
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void DeleteAll()
    {

        throw new NotImplementedException();
    }

    public Assignment? Read(int id)
    {
        return (DataSource.Assignments.Find(x => x.Id == id));
    }

    public List<Assignment> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Assignment item)
    {
        throw new NotImplementedException();
    }
}
