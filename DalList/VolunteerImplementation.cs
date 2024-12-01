
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {

        //if(DataSource.Volunteers.Find(v=> v.Id==item.Id) != null)
        if (Read(item.Id) != null)
            throw new Exception($"volunteer with id {item.Id} already exists");
        else
            DataSource.Volunteers.Add(item);
    }

    public void Delete(int id)
    {
        Volunteer? currentVolunteer = Read(id);
        if (currentVolunteer != null)
        {
            DataSource.Volunteers.Remove(currentVolunteer);
        }
        else
            throw new Exception($"volunteer with id {id} no exists");
    }

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
       // return DataSource.Volunteers.Find(v => v.Id == id);
        return DataSource.Volunteers.FirstOrDefault(item => item.Id == id); //stage 2
    }

    //public List<Volunteer> ReadAll()
    //{

    //    List<Volunteer> s_volunteer = new List<Volunteer>();
    //    foreach (var volunteer in DataSource.Volunteers)
    //        s_volunteer.Add(volunteer);
    //    return s_volunteer;
    //}

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) => filter == null
? DataSource.Volunteers.Select(item => item)
: DataSource.Volunteers.Where(filter);


    public void Update(Volunteer item)
    {
        Volunteer? itemWithId = Read(item.Id);
        if (itemWithId != null)
        {
            DataSource.Volunteers.Remove(itemWithId);
            DataSource.Volunteers.Add(item);
        }
        else
            throw new Exception($"volunteer with id {item.Id} no exists");
    }
}
