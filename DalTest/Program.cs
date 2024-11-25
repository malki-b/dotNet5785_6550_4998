using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {


        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1

        private enum MainMenu
        {
            Exit,
            VolunteerMenu,
            CallMenu,
            AssignmentMenu,
            InitializeData,
            DisplayAllData,
            ConfigMenu,
            ResetDatabase
        }
        private enum Option
        {
            Exit,
            Create,
            Read,
            ReadAll,
            UpDate,
            Delete,
            DeleteAll
        }
        private enum ConfigSubmenu
        {
            Exit,
            AdvanceClockByMinute,
            AdvanceClockByHour,
            AdvanceClockByDay,
            AdvanceClockByMonth,
            AdvanceClockByYear,
            DisplayClock,
            ChangeClockOrRiskRange,
            DisplayConfigVar,
            Reset
        }
    }

    
}
   
}

