namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using Helpers;
using System;

internal class AdminImplementation : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void ForwardClock(TimeUnit unit)
    {
        switch (unit)
        {
            case TimeUnit.Minute:
                AdminManager.UpdateClock(AdminManager.Now.AddMinutes(1));
                break;
            case TimeUnit.Hour:
                AdminManager.UpdateClock(AdminManager.Now.AddHours(1));
                break;
            case TimeUnit.Day:
                AdminManager.UpdateClock(AdminManager.Now.AddDays(1));
                break;
            case TimeUnit.Month:
                AdminManager.UpdateClock(AdminManager.Now.AddMonths(1));
                break;
            case TimeUnit.Year:
                AdminManager.UpdateClock(AdminManager.Now.AddYears(1));
                break;
            default:
                throw new ArgumentException("Invalid TimeUnit", nameof(unit));
        }
    }

    public DateTime GetClock()
    {
       // ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
        return AdminManager.Now;    
    }
    public void SetMaxRange(TimeSpan riskTimeRange)
    {
       AdminManager.MaxRange = riskTimeRange;
    }
    public TimeSpan GetMaxRange()
    {
        return AdminManager.MaxRange;
    }

    public void InitializeDB()
    {
        AdminManager.InitializeDB();
        DalTest.Initialization.Do();
        AdminManager.UpdateClock(AdminManager.Now);
    }

    public void ResetDB()
    {
        AdminManager.ResetDB();
        //
        GetClock();
    }

    
}
