
using DalApi;

namespace Dal;

//A class that will inherit and implement the IDal interface by initializing the subinterfaces in the access classes that we just implemented. Working with XML.
public class DalXml : IDal
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
