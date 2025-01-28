using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalApi;
/// <summary>
/// Defining a new interface called IDal, which will contain a separate feature for each type of subinterface.
/// 
/// </summary>
public interface IDal
{
    IVolunteer Volunteer { get; }
    ICall Call { get; }
    IAssignment Assignment { get; }
    IConfig Config { get; }
    void ResetDB();
}
