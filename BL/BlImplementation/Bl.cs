using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    public IVolunteer Volunteer => throw new NotImplementedException();

    public ICall Call => throw new NotImplementedException();

    public IAdmin Admin => throw new NotImplementedException();
}