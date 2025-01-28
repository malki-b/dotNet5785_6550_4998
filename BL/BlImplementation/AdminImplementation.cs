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
        throw new NotImplementedException();
    }

    public DateTime GetClock()
    {
        ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
        return ClockManager.Now;    
    }

    public int GetMaxRange()
    {
        throw new NotImplementedException();
    }

    public void InitializeDB()
    {
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);
    }

    public void ResetDB()
    {
        IDal.resetDB();
        GetClock();
    }

    public void SetMaxRange(int maxRange)
    {
        throw new NotImplementedException();
    }
}
