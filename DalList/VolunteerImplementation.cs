
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    internal void Create(Volunteer item)
    {

        //if(DataSource.Volunteers.Find(v=> v.Id==item.Id) != null)
        if (Read(item.Id) != null)
            throw new Exception($"volunteer with id {item.Id} already exists");
        else
            DataSource.Volunteers.Add(item);
    }

    internal void Delete(int id)
    {
        Volunteer? currentVolunteer = Read(id);
        if (currentVolunteer != null)
        {
            DataSource.Volunteers.Remove(currentVolunteer);
        }
        else
            throw new Exception($"volunteer with id {id} no exists");
    }

    internal void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    internal Volunteer? Read(int id)
    {
        return DataSource.Volunteers.Find(v => v.Id == id);

    }

    public List<Volunteer> ReadAll()
    {

        List<Volunteer> s_volunteer = new List<Volunteer>();
        foreach (var volunteer in DataSource.Volunteers)
            s_volunteer.Add(volunteer);
        return s_volunteer;
    }

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
