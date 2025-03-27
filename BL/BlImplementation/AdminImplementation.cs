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
                ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
                break;
            case TimeUnit.Hour:
                ClockManager.UpdateClock(ClockManager.Now.AddHours(1));
                break;
            case TimeUnit.Day:
                ClockManager.UpdateClock(ClockManager.Now.AddDays(1));
                break;
            case TimeUnit.Month:
                ClockManager.UpdateClock(ClockManager.Now.AddMonths(1));
                break;
            case TimeUnit.Year:
                ClockManager.UpdateClock(ClockManager.Now.AddYears(1));
                break;
            default:
                throw new ArgumentException("Invalid TimeUnit", nameof(unit));
        }
    }

    public DateTime GetClock()
    {
       // ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
        return ClockManager.Now;    
    }
    public void SetMaxRange(TimeSpan riskTimeRange)
    {
        _dal.Config.RiskRange = riskTimeRange;
    }
    public TimeSpan GetMaxRange()
    {
        return _dal.Config.RiskRange;
    }

    public void InitializeDB()
    {
        _dal.Config.Reset();
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);
    }

    public void ResetDB()
    {
        _dal.Config.Reset();
        //
        GetClock();
    }

    
}
