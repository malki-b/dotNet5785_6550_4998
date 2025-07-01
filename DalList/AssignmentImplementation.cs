

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class AssignmentImplementation : IAssignment
{
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void Create(Assignment item)
    {
        int idCall = Config.NextAssignmentId;
        Assignment copy = item with { Id = idCall };
        DataSource.Assignments.Add(copy);

    }
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void Delete(int id)
    {
        Assignment? assignment = Read(id);
        if (assignment != null)
        {
            DataSource.Assignments.Remove(assignment);

        }
        else
            throw new DalDoesNotExistException($"Assignment with id {id} no exists");
    }
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }
    [MethodImpl(MethodImplOptions.Synchronized)]

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
    [MethodImpl(MethodImplOptions.Synchronized)]

    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return DataSource.Assignments.FirstOrDefault(filter);
    }

    //    public List<Assignment> ReadAll()
    //{
    //    return DataSource.Assignments.ToList();

    //}
    [MethodImpl(MethodImplOptions.Synchronized)]

    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) => filter == null
? DataSource.Assignments.Select(item => item)
: DataSource.Assignments.Where(filter);

    [MethodImpl(MethodImplOptions.Synchronized)]

    public void Update(Assignment item)
    {
        //Delete(item.Id);
        //DataSource.Assignments.Add(item);

        Assignment? itemWithId = Read(item.Id);
        if (itemWithId != null)
        {

            DataSource.Assignments.Remove(item);
            DataSource.Assignments.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Assignment with id {item.Id} no exists");
    }
}
