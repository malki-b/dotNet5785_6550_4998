namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddVolunteer(Volunteer volunteer)
    {
        throw new NotImplementedException();
    }

    public Role Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public void RequestDeleteVolunteer(int volunteerId)
    {
        throw new NotImplementedException();
    }

    public Volunteer RequestVolunteerDetails(int volunteerId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<VolunteerInList> RequestVolunteersList(bool? isActive, VolunteerSortBy? sortBy)
    {
        throw new NotImplementedException();
    }

    public void UpdateVolunteerDetails(int requesterId, Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
