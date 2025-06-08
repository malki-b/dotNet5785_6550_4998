using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;
internal class VolunteerCollection : IEnumerable
{
    //    static readonly IEnumerable<BO.VolunteerSortBy> s_VolunteerCollection =
    //(Enum.GetValues(typeof(BO.VolunteerSortBy)) as IEnumerable<BO.VolunteerSortBy>)!;
    static readonly IEnumerable<BO.VolunteerField> s_VolunteerCollection =
        (Enum.GetValues(typeof(BO.VolunteerField)) as IEnumerable<BO.VolunteerField>)!;
    public IEnumerator GetEnumerator() => s_VolunteerCollection.GetEnumerator();

}

internal class RolesCollection : IEnumerable
{
    static readonly IEnumerable<BO.Role> s_RolesCollection =(Enum.GetValues(typeof(BO.Role)) as IEnumerable<BO.Role>)!;
    public IEnumerator GetEnumerator() => s_RolesCollection.GetEnumerator();
}
internal class DistanceCollection : IEnumerable
{
    static readonly IEnumerable<BO.TypeDistance> s_DistanceCollection = (Enum.GetValues(typeof(BO.TypeDistance)) as IEnumerable<BO.TypeDistance>)!;
    public IEnumerator GetEnumerator() => s_DistanceCollection.GetEnumerator();
}

//public enum TypeDistance
//{
//    Air,
//    Walking,
//    Drive
//}
//public enum Role
//{
//    Volunteer,
//    Manager,
//}
