using BlApi;
using BlImplementation;
using BO;
using DalApi;
using DO;

//internal class Program
//{
//    //public void Main()
//    //{
//    //    s_bl.Call.RequestCallCounts();
//    //}
//    //static readonly IDal s_dal = new DalList(); //stage 2
//    //static readonly IDal s_dal = new DalXml(); //stage 3
//    //static readonly IDal s_dal = DalApi.Factory.Get; //stage 4
//    //static readonly IBl s_bl = BlApi.Factory.Get(); //stage 4
//    //enum MainMenu
//    //{
//    //    Exit, DisplayVolunteer, DisplayAssignments, DisplayCalls, DisplayConfig, InitializeData, ResetDatabase, DisplayAllData
//    //}
//    //enum Crud
//    //{
//    //    Exit, Create, Read, ReadAll, Update, Delete, DeleteAll
//    //}
//    //enum Config
//    //{
//    //    Exit, AddClockMinute, AddClockHour, AddClockByDay, AddClockByMonth, AddClockByYear, ShowCurrentClock, ChangeClock, ShowCurrentRiskRange, ResetConfig
//    //}

//    //public static void DisplayMainMenu()
//    //{
//    //    foreach (MainMenu choice in Enum.GetValues(typeof(MainMenu)))
//    //    {
//    //        Console.WriteLine($"press {(int)choice} to {choice}");
//    //    };

//    //}

//    //public static void DisplayCrud()
//    //{
//    //    foreach (Crud choice in Enum.GetValues(typeof(Crud)))
//    //    {
//    //        Console.WriteLine($"press {(int)choice} to {choice}");
//    //    };
//    //}


////    private static void AddCall()
////    {
////        try
////        {
////            Console.WriteLine("Add Call Screen");

////            // Collecting call details from the user
////            Console.Write("Enter the call description: ");
////            string description = Console.ReadLine();

////            Console.Write("Enter the address for the call: ");
////            string address = Console.ReadLine();

////            Console.Write("Enter the call type: ");
////            TypeOfReading callType = (TypeOfReading)Enum.Parse(typeof(TypeOfReading), Console.ReadLine());


////            // Collecting Latitude and Longitude from the user
////            Console.Write("Enter the Latitude: ");
////            double latitude = double.Parse(Console.ReadLine());

////            Console.Write("Enter the Longitude: ");
////            double longitude = double.Parse(Console.ReadLine());

////            // Collecting call type
////            /* Console.Write("Enter the call type (e.g., Emergency, Routine): ");
////             string callTypeInput = Console.ReadLine();
////             CallTypeEnum callType;
////             if (!Enum.TryParse(callTypeInput, true, out callType))
////             {
////                 Console.WriteLine("Invalid call type. Setting to 'Routine' by default.");
////                 callType = CallTypeEnum.Routine;  // Set a default value if the input is invalid
////             }*/

////            // Collecting the call status
////            Console.Write("Enter the call status (e.g., Open, Closed): ");
////            string statusInput = Console.ReadLine();
////            Status status;
////            if (!Enum.TryParse(statusInput, true, out status))
////            {
////                Console.WriteLine("Invalid status. Setting to 'Open' by default.");
////                status = Status.Open;  // Set a default value if the input is invalid
////            }

////            // Set MaxEndTime (Optional)
////            DateTime? maxEnd = null;
////            Console.Write("Enter the MaxEndTime (leave blank if none): ");
////            string maxEndTimeInput = Console.ReadLine();
////            if (!string.IsNullOrEmpty(maxEndTimeInput))
////            {
////                if (DateTime.TryParse(maxEndTimeInput, out DateTime parsedMaxEndTime))
////                {
////                    maxEnd = parsedMaxEndTime;
////                }
////                else
////                {
////                    Console.WriteLine("Invalid MaxEndTime format.");
////                }
////            }


////            BO.Call newCall = new BO.Call
////            {
////                Description = description,
////                TypeOfReading = callType,
////                Address = address,
////                Latitude = latitude,
////                Longitude = longitude,
////                OpeningTime = DateTime.Now, // Current time as OpeningTime
////                MaxEndTime = maxEnd,
////                CallStatus = status,
////                CallAssignments = new List<BO.CallAssignInList>()
////            };
////            s_bl.Call.Create(newCall);


////            Console.WriteLine("New call added successfully!");
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"An error occurred while adding the call: {ex.Message}");
////        }
////    }
////}

using System;
using BlApi;
using BO;
using System.Xml.Linq;
using DalTest;

namespace BLTest
{
    class Program
    {
        static readonly IDal s_dal = DalApi.Factory.Get; //stage 4
        static readonly IBl s_bl = BlApi.Factory.Get(); //stage 4

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nLogin Menu:");
                Console.Write("Enter username: ");
                string username = Console.ReadLine() ?? "";
                Console.Write("Enter password: ");
                string password = Console.ReadLine() ?? "";

               // Assuming Login method returns role as string
                BO.Role? role = null;
                try
                {
                    role = s_bl.Volunteer.Login(username, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (role == null)
                {
                    Console.WriteLine("Invalid login credentials. Please try again.");
                    continue;
                }

                Console.WriteLine("Main Menu:");
                Console.WriteLine("Press 1 for Volunteer Menu");
                Console.WriteLine("Press 2 for Call Menu");
                Console.WriteLine("Press 3 for Admin Menu");
                Console.WriteLine("Press 0 to Exit");
                Console.Write("Select an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 0:
                            return;
                        case 1:
                            VolunteerMenu();
                            break;
                        case 2:
                            CallMenu();
                            break;
                        case 3:
                            AdminMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void VolunteerMenu()
        {
            while (true)
            {
                Console.WriteLine("Volunteer Menu:");
                Console.WriteLine("1. Add Volunteer");
                Console.WriteLine("2. Read Volunteer");
                Console.WriteLine("3. Update Volunteer");
                Console.WriteLine("4. Delete Volunteer");
                Console.WriteLine("5. ReadAll Volunteers");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Select an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddVolunteer();
                            break;
                        case 2:
                            ReadVolunteer();
                            break;
                        case 3:
                            UpdateVolunteer();
                            break;
                        case 4:
                            DeleteVolunteer();
                            break;
                        case 5:
                            ReadAllVolunteers();
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void AddVolunteer()
        {
            try
            {
                Console.Write("Enter Volunteer ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Full Name: ");
                string fullName = Console.ReadLine();

                Console.Write("Enter Phone: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Console.Write("Enter Type Distance: ");
                if (!Enum.TryParse(Console.ReadLine(), out BO.TypeDistance typeDistance))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Type Distance.");
                    return;
                }

                Console.Write("Enter Role: ");
                if (!Enum.TryParse(Console.ReadLine(), out BO.Role role))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Role.");
                    return;
                }

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Latitude: ");
                if (!double.TryParse(Console.ReadLine(), out double latitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Longitude: ");
                if (!double.TryParse(Console.ReadLine(), out double longitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Max Distance: ");
                if (!double.TryParse(Console.ReadLine(), out double maxDistance))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                //BO.Volunteer volunteer = new BO.Volunteer(id, fullName, phone, email, password, typeDistance, role, address, latitude, longitude, true, maxDistance);

                int totalHandledCalls = 0;
                int totalCanceledCalls = 0;
                int totalExpiredHandledCalls = 0;
                BO.CallInProgress? currentCallInProgress = null;

                BO.Volunteer volunteer = new BO.Volunteer(
                    id,
                    fullName,
                    phone,
                    email,
                    password,
                    typeDistance,
                    role,
                    address,
                    latitude,
                    longitude,
                    true,
                    maxDistance,
                    totalHandledCalls,
                    totalCanceledCalls,
                    totalExpiredHandledCalls,
                    currentCallInProgress // Updated line to include new parameters
                );

                s_bl.Volunteer.Create(volunteer);
                Console.WriteLine("Volunteer added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ReadVolunteer()
        {

            try
            {
                var volunteers = s_bl.Volunteer.ReadAll();
                foreach (var volunteer in volunteers)
                {
                    //Console.WriteLine($"ID: {volunteer.VolunteerId}, Name: {volunteer.FullName}, IsActive: {volunteer.IsActive}");
                    Console.WriteLine(volunteer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }




            try
            {
                Console.Write("Enter Volunteer ID: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                BO.Volunteer volunteer = s_bl.Volunteer.Read(id);
                if (volunteer == null)
                {
                    Console.WriteLine("Volunteer not found.");
                    return;
                }
                Console.WriteLine(volunteer.ToString());
                //Console.WriteLine($"ID: {volunteer.Id}");
                //Console.WriteLine($"Name: {volunteer.FullName}");
                //Console.WriteLine($"Phone: {volunteer.Phone}");
                //Console.WriteLine($"Email: {volunteer.Email}");
                //Console.WriteLine($"Type Distance: {volunteer.TypeDistance}");
                //Console.WriteLine($"Role: {volunteer.Role}");
                //Console.WriteLine($"Address: {volunteer.Address}");
                //Console.WriteLine($"Latitude: {volunteer.Latitude}");
                //Console.WriteLine($"Longitude: {volunteer.Longitude}");
                //Console.WriteLine($"Max Distance: {volunteer.MaxDistance}");
                //Console.WriteLine($"Is Active: {volunteer.IsActive}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private static void UpdateVolunteer()
        {
            try
            {
                Console.Write("Enter Volunteer ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Full Name: ");
                string fullName = Console.ReadLine();

                Console.Write("Enter Phone: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Console.Write("Enter Type Distance: ");
                if (!Enum.TryParse(Console.ReadLine(), out BO.TypeDistance typeDistance))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Type Distance.");
                    return;
                }

                Console.Write("Enter Role: ");
                if (!Enum.TryParse(Console.ReadLine(), out BO.Role role))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Role.");
                    return;
                }

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Latitude: ");
                if (!double.TryParse(Console.ReadLine(), out double latitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Longitude: ");
                if (!double.TryParse(Console.ReadLine(), out double longitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Max Distance: ");
                if (!double.TryParse(Console.ReadLine(), out double maxDistance))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                // Assuming default values for the new fields as they are required in the constructor
                int totalHandledCalls = 0;
                int totalCanceledCalls = 0;
                int totalExpiredHandledCalls = 0;
                BO.CallInProgress? currentCallInProgress = null;

                BO.Volunteer volunteer = new BO.Volunteer(
                    id,
                    fullName,
                    phone,
                    email,
                    password,
                    typeDistance,
                    role,
                    address,
                    latitude,
                    longitude,
                    true,
                    maxDistance,
                    totalHandledCalls,
                    totalCanceledCalls,
                    totalExpiredHandledCalls,
                    currentCallInProgress // Updated line to include new parameters
                );

                s_bl.Volunteer.Update(id, volunteer);
                Console.WriteLine("Volunteer updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private static void DeleteVolunteer()
        {
            try
            {
                Console.Write("Enter Volunteer ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                s_bl.Volunteer.Delete(id);
                Console.WriteLine("Volunteer deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ReadAllVolunteers()
        {
            try
            {
                var volunteers = s_bl.Volunteer.ReadAll();
                foreach (var volunteer in volunteers)
                {
                    //Console.WriteLine($"ID: {volunteer.VolunteerId}, Name: {volunteer.FullName}, IsActive: {volunteer.IsActive}");
                    Console.WriteLine(volunteer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void CallMenu()
        {
            while (true)
            {
                Console.WriteLine("Call Menu:");
                Console.WriteLine("1. Add a Call");
                Console.WriteLine("2. View Call Details");
                Console.WriteLine("3. Update Call");
                Console.WriteLine("4. Delete Call");
                Console.WriteLine("5. List Calls");
                Console.WriteLine("6. Request Call Counts");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Select an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddCall();
                            break;
                        case 2:
                            RequestCallDetails();
                            break;
                        case 3:
                            UpdateCall();
                            break;
                        case 4:
                            DeleteCall();
                            break;
                        case 5:
                            ReadAllCalls();
                            break;
                        case 6:
                            RequestCallCounts();
                            break;
                        case 7:
                            RequestClosedCallsByVolunteer();
                            break;
                        case 8:
                            RequestOpenCallsForSelection();
                            break;
                        case 9:
                            UpdateCallCompletion();
                            break;
                        case 10:
                            UpdateCallCancellation();
                            break;
                        case 11:
                            SelectCallForTreatment();
                            break;
                        case 12:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        private static void AddCall()
        {
            try
            {
                Console.Write("Enter Call ID: ");
                if (!int.TryParse(Console.ReadLine(), out int callId))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Latitude: ");
                if (!double.TryParse(Console.ReadLine(), out double latitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Longitude: ");
                if (!double.TryParse(Console.ReadLine(), out double longitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Opening Time: ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime openingTime))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                    return;
                }

                Console.Write("Enter Type of Reading: ");
                if (!Enum.TryParse(Console.ReadLine(), out BO.TypeOfReading typeOfReading))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Type of Reading.");
                    return;
                }

                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Console.Write("Enter Max End Time (optional): ");
                DateTime? maxEndTime = null;
                string maxEndTimeInput = Console.ReadLine();
                if (!DateTime.TryParse(maxEndTimeInput, out DateTime tempMaxEndTime))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                    return;
                }



                BO.Call call = new BO.Call
                {
                    CallId = callId,
                    Address = address,
                    Latitude = latitude,
                    Longitude = longitude,
                    OpeningTime = openingTime,
                    TypeOfReading = typeOfReading,
                    Description = description,
                    MaxEndTime = tempMaxEndTime,
                    CallStatus = BO.Status.Open,
                    CallAssignments = new List<BO.CallAssignInList>()
                };

                s_bl.Call.Create(call);
                Console.WriteLine("Call created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void RequestCallDetails()
        {
            try
            {
                Console.Write("Enter Call ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                BO.Call call = s_bl.Call.RequestCallDetails(id);
                if (call == null)
                {
                    Console.WriteLine("Call not found.");
                    return;
                }
                Console.WriteLine(call.ToString());
                //Console.WriteLine($"ID: {call.CallId}");
                //Console.WriteLine($"Address: {call.Address}");
                //Console.WriteLine($"Latitude: {call.Latitude}");
                //Console.WriteLine($"Longitude: {call.Longitude}");
                //Console.WriteLine($"Opening Time: {call.OpeningTime}");
                //Console.WriteLine($"Type of Reading: {call.TypeOfReading}");
                //Console.WriteLine($"Description: {call.Description}");
                //Console.WriteLine($"Max End Time: {call.MaxEndTime}");
                //Console.WriteLine($"Status: {call.CallStatus}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void UpdateCall()
        {
            try
            {
                Console.Write("Enter Call ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Latitude: ");
                if (!double.TryParse(Console.ReadLine(), out double latitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Longitude: ");
                if (!double.TryParse(Console.ReadLine(), out double longitude))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                Console.Write("Enter Opening Time: ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime openingTime))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                    return;
                }

                Console.Write("Enter Type of Reading: ");
                if (!Enum.TryParse(Console.ReadLine(), out BO.TypeOfReading typeOfReading))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Type of Reading.");
                    return;
                }

                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Console.Write("Enter Max End Time (optional): ");
                DateTime? maxEndTime = null;
                string maxEndTimeInput = Console.ReadLine();
                if (!DateTime.TryParse(maxEndTimeInput, out DateTime tempMaxEndTime))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                    return;
                }

                BO.Call call = new BO.Call
                {
                    CallId = id,
                    Address = address,
                    Latitude = latitude,
                    Longitude = longitude,
                    OpeningTime = openingTime,
                    TypeOfReading = typeOfReading,
                    Description = description,
                    MaxEndTime = tempMaxEndTime,
                    CallStatus = BO.Status.Open,
                    CallAssignments = new List<BO.CallAssignInList>() // Assuming this is correct
                };

                s_bl.Call.UpdateCallDetails(call);
                Console.WriteLine("Call updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void DeleteCall()
        {
            try
            {
                Console.Write("Enter Call ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    return;
                }

                s_bl.Call.Delete(id);
                Console.WriteLine("Call deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ReadAllCalls()
        {
            try
            {
                Console.WriteLine("please enter the option  this  Id,\r\n    CallType,\r\n    Description,\r\n    FullAddress,\r\n    Latitude,\r\n    Longitude,\r\n    OpeningTime,\r\n    MaxEndTime,\r\n    CallStatus,");
                var filterBy = Console.ReadLine();
                  //  BO.CallField? filterBy, object? filterValue, BO.CallField? sortBy
                IEnumerable<CallInList> list = s_bl.Call.ReadAll(null , null , null);/////////////////

                foreach (CallInList call in list)
                {
                    Console.WriteLine(call.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void RequestCallCounts()
        {
            try
            {
                int[] counts = s_bl.Call.RequestCallCounts();
                Console.WriteLine("Call Counts:");
                Console.WriteLine($"Open: {counts[0]}");
                Console.WriteLine($"In Progress: {counts[1]}");
                Console.WriteLine($"Closed: {counts[2]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private static void RequestClosedCallsByVolunteer()
        {
            Console.Write("Enter Volunteer ID: ");
            int volunteerId = int.Parse(Console.ReadLine());

            Console.Write("Enter filter type (optional): ");
            string filterTypeInput = Console.ReadLine();
            BO.TypeOfReading? filterType = string.IsNullOrEmpty(filterTypeInput) ? (BO.TypeOfReading?)null : Enum.Parse<BO.TypeOfReading>(filterTypeInput);

            Console.Write("Enter sort field (optional): ");
            string sortFieldInput = Console.ReadLine();
            CallField? sortField = string.IsNullOrEmpty(sortFieldInput) ? (CallField?)null : Enum.Parse<CallField>(sortFieldInput);

            var closedCalls = new CallImplementation().RequestClosedCallsByVolunteer(volunteerId, filterType, sortField);

            foreach (var call in closedCalls)
            {
                Console.WriteLine($"Call ID: {call.CallId}, Status: {call.Status}, Volunteer: {call.VolunteerName}");
            }
        }

        private static void RequestOpenCallsForSelection()
        {
            Console.Write("Enter Volunteer ID: ");
            int volunteerId = int.Parse(Console.ReadLine());

            Console.Write("Enter filter type (optional): ");
            string filterTypeInput = Console.ReadLine();
            BO.TypeOfReading? filterType = string.IsNullOrEmpty(filterTypeInput) ? (BO.TypeOfReading?)null : Enum.Parse<BO.TypeOfReading>(filterTypeInput);

            Console.Write("Enter sort field (optional): ");
            string sortFieldInput = Console.ReadLine();
            CallField? sortField = string.IsNullOrEmpty(sortFieldInput) ? (CallField?)null : Enum.Parse<CallField>(sortFieldInput);

            var openCalls = new CallImplementation().RequestOpenCallsForSelection(volunteerId, filterType, sortField);

            foreach (var call in openCalls)
            {
                Console.WriteLine($"Call ID: {call.CallId}, Status: {call.Status}, Volunteer: {call.VolunteerName}");
            }
        }

        private static void UpdateCallCompletion()
        {
            Console.Write("Enter Volunteer ID: ");
            int volunteerId = int.Parse(Console.ReadLine());

            Console.Write("Enter Assignment ID: ");
            int assignmentId = int.Parse(Console.ReadLine());

            new CallImplementation().UpdateCallCompletion(volunteerId, assignmentId);

            Console.WriteLine("Call completion updated successfully.");
        }

        private static void UpdateCallCancellation()
        {
            Console.Write("Enter Requester ID: ");
            int requesterId = int.Parse(Console.ReadLine());

            Console.Write("Enter Assignment ID: ");
            int assignmentId = int.Parse(Console.ReadLine());

            new CallImplementation().UpdateCallCancellation(requesterId, assignmentId);

            Console.WriteLine("Call cancellation updated successfully.");
        }

        private static void SelectCallForTreatment()
        {
            Console.Write("Enter Volunteer ID: ");
            int volunteerId = int.Parse(Console.ReadLine());

            Console.Write("Enter Call ID: ");
            int callId = int.Parse(Console.ReadLine());

            new CallImplementation().SelectCallForTreatment(volunteerId, callId);

            Console.WriteLine("Call selected for treatment successfully.");
        }

        private static void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Reset DB");
                Console.WriteLine("2. Initialize DB");
                Console.WriteLine("3. Set Max Range");
                Console.WriteLine("4. Get Max Range");
                Console.WriteLine("5. Forward Clock");
                Console.WriteLine("6. Get Clock");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Select an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            s_dal.ResetDB();
                            Console.WriteLine("Database reset.");
                            break;
                        case 2:
                            Initialization.Do();
                            Console.WriteLine("Database initialized.");
                            break;
                        case 3:
                            Console.Write("Enter max range (hh:mm:ss): ");
                            if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan maxRange))
                            {
                                s_bl.Admin.SetMaxRange(maxRange);
                                Console.WriteLine("Max range set.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid max range.");
                            }
                            break;
                        case 4:
                            Console.WriteLine($"Max range: {s_bl.Admin.GetMaxRange()}");
                            break;
                        case 5:
                            Console.Write("Enter time unit (e.g., HOUR): ");
                            if (Enum.TryParse(Console.ReadLine(), out TimeUnit timeUnit))
                            {
                                s_bl.Admin.ForwardClock(timeUnit);
                                Console.WriteLine("Clock forwarded.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid time unit.");
                            }
                            break;
                        case 6:
                            Console.WriteLine($"Current clock: {s_bl.Admin.GetClock()}");
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}




//private static void UpdateVolunteer()
//{
//    try
//    {
//        Console.Write("Enter Volunteer ID: ");
//        if (!int.TryParse(Console.ReadLine(), out int id))
//        {
//            Console.WriteLine("Invalid input. Please enter a number.");
//            return;
//        }

//        Console.Write("Enter Full Name: ");
//        string fullName = Console.ReadLine();

//        Console.Write("Enter Phone: ");
//        string phone = Console.ReadLine();

//        Console.Write("Enter Email: ");
//        string email = Console.ReadLine();

//        Console.Write("Enter Password: ");
//        string password = Console.ReadLine();

//        Console.Write("Enter Type Distance: ");
//        if (!Enum.TryParse(Console.ReadLine(), out BO.TypeDistance typeDistance))
//        {
//            Console.WriteLine("Invalid input. Please enter a valid Type Distance.");
//            return;
//        }

//        Console.Write("Enter Role: ");
//        if (!Enum.TryParse(Console.ReadLine(), out BO.Role role))
//        {
//            Console.WriteLine("Invalid input. Please enter a valid Role.");
//            return;
//        }

//        Console.Write("Enter Address: ");
//        string address = Console.ReadLine();

//        Console.Write("Enter Latitude: ");
//        if (!double.TryParse(Console.ReadLine(), out double latitude))
//        {
//            Console.WriteLine("Invalid input. Please enter a number.");
//            return;
//        }

//        Console.Write("Enter Longitude: ");
//        if (!double.TryParse(Console.ReadLine(), out double longitude))
//        {
//            Console.WriteLine("Invalid input. Please enter a number.");
//            return;
//        }

//        Console.Write("Enter Max Distance: ");
//        if (!double.TryParse(Console.ReadLine(), out double maxDistance))
//        {
//            Console.WriteLine("Invalid input. Please enter a number.");
//            return;
//        }

//        BO.Volunteer volunteer = new BO.Volunteer(id, fullName, phone, email, password, typeDistance, role, address, latitude, longitude, true, maxDistance);


//        s_bl.Volunteer.Update(id, volunteer);
//        Console.WriteLine("Volunteer updated successfully.");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error: {ex.Message}");
//    }
//}



