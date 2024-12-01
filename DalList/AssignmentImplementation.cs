

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class AssignmentImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        int idCall = Config.NextAssignmentId;
        Assignment copy = item with { Id = idCall };
        DataSource.Assignments.Add(copy);

    }

    public void Delete(int id)
    {
        Assignment? assignment = Read(id);
        if (assignment != null)
        {
            DataSource.Assignments.Remove(assignment);

        }
        else
            throw new Exception($"Assignment with id {id} no exists");
    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        //return (DataSource.Assignments.Find(x => x.Id == id));
        return DataSource.Assignments.FirstOrDefault(item => item.Id == id); //stage 2
    }

    //public Assignment? Read(Func<Assignment, bool> filter)
    //{
    //    return DataSource.Assignments.FirstOrDefault(item => item); //stage 2
    //    throw new NotImplementedException();
    //}

    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return DataSource.Assignments.FirstOrDefault(filter);
    }

    //    public List<Assignment> ReadAll()
    //{
    //    return DataSource.Assignments.ToList();

    //}

    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) => filter == null
? DataSource.Assignments.Select(item => item)
: DataSource.Assignments.Where(filter);


    public void Update(Assignment item)
    {
        Delete(item.Id);
        DataSource.Assignments.Add(item);
    }
}
