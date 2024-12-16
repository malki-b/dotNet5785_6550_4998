

namespace DalApi;
using DO;
using System;
using System.Collections.Generic;

public interface IAssignment : ICrud<Assignment>
{
    IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null);
}
