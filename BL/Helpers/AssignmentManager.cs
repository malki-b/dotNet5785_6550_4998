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

    internal static ObserverManager Observers = new(); //stage 5

    internal static bool VolunteerIsOnCall(int volunteerId)
    {
        lock (AdminManager.BlMutex)
        {
            DO.Assignment? doLink = s_dal.Assignment.Read(l => l.VolunteerId == volunteerId);
            return doLink != null; // Return true if the volunteer is on call, false otherwise
        }
    }
}
