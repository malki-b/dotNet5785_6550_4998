
namespace Dal;
using DalApi;
/// <summary>
/// A class that will inherit and implement the new interface IDal by 
/// initializing the subinterfaces in the access classes that we implemented in step 1.
/// </summary>
sealed public class DalList : IDal
{
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();

    public ICall Call { get; } = new CallImplementation();


    public IAssignment Assignment { get; } = new AssignmentImplementation();

    public IConfig Config { get; } = new ConfigImplementation();


    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}

