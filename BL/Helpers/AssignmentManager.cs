using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

internal static class AssignmentManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    internal static VolunteerIsOnCall(int volunteerId)
    {
        DO.Assignment? doLink = s_dal.Assignment.Read(l => l.VolunteerId == volunteerId)
          ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={volunteerId} does Not take Call");
    }


}
