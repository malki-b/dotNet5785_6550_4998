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
        lock (AdminManager.BlMutex)
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
    }

    public DateTime GetClock()
    {
        // Reading the clock is not a DAL operation, but for consistency, you may lock it as well:
        lock (AdminManager.BlMutex)
        {
            return AdminManager.Now;
        }
    }

    public void SetMaxRange(TimeSpan riskTimeRange)
    {
        lock (AdminManager.BlMutex)
        {
            AdminManager.MaxRange = riskTimeRange;
        }
    }

    public TimeSpan GetMaxRange()
    {
        lock (AdminManager.BlMutex)
        {
            return AdminManager.MaxRange;
        }
    }

    public void InitializeDB()
    {
        lock (AdminManager.BlMutex)
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            AdminManager.InitializeDB();
            DalTest.Initialization.Do();
            AdminManager.UpdateClock(AdminManager.Now);
        }
    }

    public void ResetDB()
    {
        lock (AdminManager.BlMutex)
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            AdminManager.ResetDB();
            GetClock();
        }
    }

    #region Stage 5
    public void AddClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers += clockObserver;
    public void RemoveClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers -= clockObserver;
    public void AddConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers += configObserver;
    public void RemoveConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers -= configObserver;
    #endregion Stage 5

    public void StartSimulator(int interval)
    {
        lock (AdminManager.BlMutex)
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            AdminManager.Start(interval);
        }
    }

    public void StopSimulator()
    {
        lock (AdminManager.BlMutex)
        {
            AdminManager.Stop();
        }
    }
}
