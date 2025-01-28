using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IVolunteer
{

    /// <summary>
    /// Method to log in to the system.
    /// </summary>
    /// <param name="username">The username of the volunteer.</param>
    /// <param name="password">The password of the volunteer.</param>
    /// <returns>The role of the user.</returns>
    public Role Login(string username, string password);

    /// <summary>
    /// Method to request a list of volunteers.
    /// </summary>
    /// <param name="isActive">Filter for active/inactive volunteers (nullable).</param>
    /// <param name="sortBy">The enumeration field to sort by (nullable).</param>
    public IEnumerable<BO.VolunteerInList> RequestVolunteersList(bool? isActive, BO.VolunteerSortField? sortBy);

    /// <summary>
    /// Method to request volunteer details.
    /// </summary>
    /// <param name="volunteerId">The ID (T.Z) of the volunteer.</param>
    /// <returns>The Volunteer entity including CallInProgress if applicable.</returns>
    public Volunteer RequestVolunteerDetails(int volunteerId);

    /// <summary>
    /// Method to update volunteer details.
    /// </summary>
    /// <param name="requesterId">The ID of the requester.</param>
    /// <param name="volunteer">The Volunteer object containing updated details.</param>
    public void UpdateVolunteerDetails(int requesterId, Volunteer volunteer);

    /// <summary>
    /// Method to request deletion of a volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID (T.Z) of the volunteer to be deleted.</param>
    public void RequestDeleteVolunteer(int volunteerId);

    /// <summary>
    /// Method to add a new volunteer.
    /// </summary>
    /// <param name="volunteer">The Volunteer object to be added.</param>
    public void AddVolunteer(Volunteer volunteer);

}
