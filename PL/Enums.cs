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
    static readonly IEnumerable<BO.VolunteerInListFields> s_VolunteerCollection =
        (Enum.GetValues(typeof(BO.VolunteerInListFields)) as IEnumerable<BO.VolunteerInListFields>)!;
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
internal class CallFieldCollection : IEnumerable
{
    static readonly IEnumerable<BO.CallField> s_CallFieldCollection = (Enum.GetValues(typeof(BO.CallField)) as IEnumerable<BO.CallField>)!;
    public IEnumerator GetEnumerator() => s_CallFieldCollection.GetEnumerator();
}

internal class TypeOfReadingCollection : IEnumerable
{
    static readonly IEnumerable<BO.TypeOfReading> s_TypeOfReadingCollection = (Enum.GetValues(typeof(BO.TypeOfReading)) as IEnumerable<BO.TypeOfReading>)!;
    public IEnumerator GetEnumerator() => s_TypeOfReadingCollection.GetEnumerator();
}
internal class OpenCallFieldCollection : IEnumerable
{
    static readonly IEnumerable<BO.OpenCallField> s_OpenCallFieldCollection = (Enum.GetValues(typeof(BO.OpenCallField)) as IEnumerable<BO.OpenCallField>)!;
    public IEnumerator GetEnumerator() => s_OpenCallFieldCollection.GetEnumerator();
}
internal class ClosedCallFieldCollection : IEnumerable
{
    static readonly IEnumerable<BO.ClosedCallField> s_ClosedCallFieldCollection = (Enum.GetValues(typeof(BO.ClosedCallField)) as IEnumerable<BO.ClosedCallField>)!;
    public IEnumerator GetEnumerator() => s_ClosedCallFieldCollection.GetEnumerator();
}
