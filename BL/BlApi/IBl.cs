using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;


public interface IBl
{
    IVoluntee1r Volunteer { get; }
    ICall Call { get; }
    IAdmin1 Admin { get; }
}
