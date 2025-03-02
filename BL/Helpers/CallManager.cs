using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get; //stage 4
    internal static Status GetCallStatus(int callId)
    {
        var call = s_dal.Call.Read(callId) ??
            throw new BO.BlDoesNotExistException($"Call with ID {callId} not found.");

        DateTime currentTime = ClockManager.Now;
        var assignments = s_dal.Assignment.ReadAll(a => a.CallId == callId).ToList();
        Assignment? activeAssignment = assignments.Find(a => a.EndOfTreatmentTime == null);
        Assignment? handledAssignments = assignments.Find(a => a.EndOfTreatmentTime != null && a.TypeOfEnding == DO.TypeOfEnding.Teated);

        if (activeAssignment != null)
        {
            if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value - s_dal.Config.RiskRange)
                return Status.Open;

            return Status.InProgress;
        }

        if (handledAssignments != null)
            return Status.Closed;

        if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value)
            return Status.Expired;

        if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value - s_dal.Config.RiskRange)
            return Status.OpenAtRisk;

        return Status.Open;
    }

    
    public static TimeSpan getRemainingTimeToEndCall(DO.Call c)
    {
        if (c.MaxTimeFinishRead.HasValue)
        {
            return c.MaxTimeFinishRead.Value - DateTime.Now;
        }
        else
        {
            return TimeSpan.Zero; // או להחזיר ערך אחר במידה ואין MaxTimeFinishRead
        }
    }
    public static TimeSpan getMaxTimeFinishRead(DO.Call c)
    {
        if (c.MaxTimeFinishRead.HasValue)
        {
            return c.MaxTimeFinishRead.Value - c.OpeningTime;
        }
        else
        {
            return TimeSpan.Zero; // או להחזיר ערך אחר במידה ואין MaxTimeFinishRead
        }
    }
}




