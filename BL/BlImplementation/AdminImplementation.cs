namespace BlImplementation;
using BlApi;
using BO;
using Helpers;
using System;

internal class AdminImplementation : IAdmin
{
    public void ForwardClock(TimeUnit unit)
    {
        throw new NotImplementedException();
    }

    public DateTime GetClock()
    {

        ClockManager.Now
        ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));
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
        throw new NotImplementedException();
    }

    public void SetMaxRange(int maxRange)
    {
        throw new NotImplementedException();
    }
}
