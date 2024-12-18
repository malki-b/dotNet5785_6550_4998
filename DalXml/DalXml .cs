using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class DalXml : IDal
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
}
