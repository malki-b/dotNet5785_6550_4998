
using DalApi;

namespace Dal;
using DalApi;
using DO;

//A class that will implement the properties and methods we defined in the IConfig interface.
internal class ConfigImplementation : IConfig
{
    public int NextAssignmentId { get => Config.NextAssignmentId; }
    public int NextCallId { get => Config.NextCallId; }
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }


    public void Reset()
    {
        Config.Reset();
    }

    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }
}