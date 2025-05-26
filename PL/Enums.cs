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
