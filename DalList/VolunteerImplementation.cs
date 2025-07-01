
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Volunteer item)
    {

        //if(DataSource.Volunteers.Find(v=> v.Id==item.Id) != null)
        if (Read(item.Id) != null)
            throw new DalAlreadyExistsException($"Volunteer with ID={item.Id} already exists");
        else
            DataSource.Volunteers.Add(item);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        Volunteer? currentVolunteer = Read(id);
        if (currentVolunteer != null)
        {
            DataSource.Volunteers.Remove(currentVolunteer);
        }
        else
            throw new DalDoesNotExistException($"Volunteer with id {id} no exists");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(int id)
    {
        // return DataSource.Volunteers.Find(v => v.Id == id);
        return DataSource.Volunteers.FirstOrDefault(item => item.Id == id); //stage 2
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return DataSource.Volunteers.FirstOrDefault(filter);
    }
    //public List<Volunteer> ReadAll()
    //{

    //    List<Volunteer> s_volunteer = new List<Volunteer>();
    //    foreach (var volunteer in DataSource.Volunteers)
    //        s_volunteer.Add(volunteer);
    //    return s_volunteer;
    //}

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) => filter == null
? DataSource.Volunteers.Select(item => item)
: DataSource.Volunteers.Where(filter);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Volunteer item)
    {
        Volunteer? itemWithId = Read(item.Id);
        if (itemWithId != null)
        {
            DataSource.Volunteers.Remove(itemWithId);
            DataSource.Volunteers.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Volunteer with id {item.Id} no exists");
    }
}


//public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) // stage 2
//{
//    // חיפוש נתונים עם או בלי סינון
//    var result = filter == null
//        ? DataSource.Volunteers
//        : DataSource.Volunteers.Where(filter);

//    // זריקת חריגה אם אין נתונים
//    if (!result.Any())
//    {
//        throw new DalReedAllImpossible("No Volunteers found.");
//    }

//    return result;
//}

