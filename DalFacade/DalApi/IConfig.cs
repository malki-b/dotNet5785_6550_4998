
namespace DalApi;
public interface IConfig
{
    DateTime Clock { get; set; }
    public TimeSpan RiskRange { get; set; }
    int NextAssignmentId { get; }
    int NextCallId { get; }

    void Reset();
}

