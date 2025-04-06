namespace BlImplementation;
using BlApi;
using BO;

using DO;
using Helpers;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Volunteer boVolunteer)
    {
        try
        {
            //ClockManager.Now, 
            //ClockManager.Now, 
            //DO.Volunteer doVolunteer = VolunteerManager.ConvertToDO(boVolunteer)
            DO.Volunteer doVolunteer =
new(boVolunteer.Id, boVolunteer.FullName, boVolunteer.Phone, boVolunteer.Email,
boVolunteer.Password, (DO.TypeDistance)boVolunteer.TypeDistance, (DO.Role)boVolunteer.Role, boVolunteer.Address, boVolunteer.Latitude,
boVolunteer.Longitude, boVolunteer.IsActive,
boVolunteer.MaxDistance);
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            if (_dal.Volunteer.Read(id) == null)
                throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");

            //var volunteer = _dal.Volunteer.Read(id)
            //    ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");

            // Check if the volunteer is handling any cases
            if (!AssignmentManager.VolunteerIsOnCall(id))
            {
                //_dal.Volunteer.Delete(id);
                //throw new BO.BlDoesNotExistException($"Failed to delete volunteer with ID={id}.", ex);

                try
                {
                    //Attempt to delete the volunteer from the data access layer
                    _dal.Volunteer.Delete(id);
                }
                catch (DO.DalNotFoundException ex)
                {
                    // Handle the case when the volunteer is not found in the data layer
                    throw new BO.BlDoesNotExistException($"Failed to delete volunteer with ID={id}.", ex);
                }
            }
            else
            {
                throw new BO.BlCannotDeleteException("Volunteer cannot be deleted because they are currently handling cases.");
            }
        }
        catch (DO.DalDoNotSuccseedDelete ex)
        {
            throw new BO.BlCannotDeleteException("Volunteer with ID already exists", ex);
        }

    }

    //public BO.StudentGradeSheet GetGradeSheetPerStudent(int studentId, BO.Year year = null)
    //{
    //    throw new NotImplementedException();
    //}

    public BO.Role Login(string username, string password)
    {
        try
        {
            var user = _dal.Volunteer.ReadAll().FirstOrDefault(u => u.Name == username);
            if (user == null || user.Password != password)
                throw new("The username or password is incorrect.");
            // return AssignmentManager.LinkStudentToCourse(VolunteerId, callId);
            return (BO.Role)user.Role;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException("Login failed.", ex);
        }
    }


    //public void UnRegisterStudentFromCourse(int studentId, int courseId)
    //{
    //    throw new NotImplementedException();
    //}

    //public void RegisterStudentToCourse(int studentId, int courseId)
    //{
    //  return  LinkManager.LinkStudentToCourse(studentId, courseId);
    //}
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.VolunteerSortBy? sortBy = null)
    {


        //var Volunteer = _dal.Volunteer.ReadAll();

        //if (isActive == null)
        //{
        //    //?? throw new BO.BlDoesNotExistException("Volunteer with ID does Not exist");
        //    return( (IEnumerable<VolunteerInList>)Volunteer).ToList();
        //}
        //else
        //{
        //    var xx = _dal.Volunteer.ReadAll().Where(v => v.IsActive == isActive.Value).ToString();
        //    return (IEnumerable<BO.VolunteerInList>)xx;

        //}
        //if (sortBy == null)
        //{
        //    var xx = _dal.Volunteer.ReadAll().Where(v => v.id == isActive.Value).ToString(); //תז

        //    return _dal.Volunteer.ReadAll(isActive);
        //}
        //else
        //{
        //    //
        //}

        //var doVolunteer = _dal.Volunteer.ReadAll(id) ??
        //      throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");

        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll(isActive is null ? null : v => v.IsActive == isActive).ToList();

        //IEnumerable<DO.Volunteer> allVolunteers = _dal.Volunteer.ReadAll().ToList();

        IEnumerable<BO.VolunteerInList> volunteersList = volunteers.Select(volunteer =>
        {
            var volunteerAssignments = _dal.Assignment.ReadAll(a => a.VolunteerId == volunteer.Id);

            int TotalHandledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
            int TotalCanceledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
            int TotalExpiredRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

            var currentCallId = volunteerAssignments.FirstOrDefault(a => a.TypeOfEnding == null)?.CallId;

            BO.TypeOfReading callType = currentCallId.HasValue ? (BO.TypeOfReading)_dal.Call.Read(currentCallId.Value)!.TypeOfReading : (BO.TypeOfReading)DO.TypeOfReading.None;

            return new BO.VolunteerInList
            {
                VolunteerId = volunteer.Id,
                FullName = volunteer.Name,
                IsActive = volunteer.IsActive ?? false,
                TotalHandledRequests = TotalHandledRequests,
                TotalCanceledRequests = TotalCanceledRequests,
                TotalExpiredRequests = TotalExpiredRequests,
                HandledRequestId = currentCallId,
                TypeOfReading = callType

            };
        });


        if (sortBy == null)
        {
            return volunteersList.OrderBy(v => v.VolunteerId).ToList(); // Sort by Id if sortBy is null
        }

        // Sorting based on the specified enum value
        var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sortBy.ToString());

        if (propertyInfo != null)
        {
            return volunteersList.OrderBy(v => propertyInfo.GetValue(v, null)).ToList();
            //return volunteersList.OrderBy(v => propertyInfo.GetValue(v)).ToList();

        }


        ////var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sortBy.ToString());
        //var x = sortBy.ToString();
        //if (propertyInfo != null)
        //{
        //    return volunteersList.OrderBy(v => v.x).ToList();
        //}
        return volunteersList;


    }

    public void Update(int requesterId, BO.Volunteer boVolunteer)
    {
        DO.Volunteer? requester = _dal.Volunteer.Read(requesterId);
        DO.Volunteer? up = _dal.Volunteer.Read(boVolunteer.Id);
        if (requester is null || requester.Role != DO.Role.Manager)
            throw new BO.BlDoesNotExistException("You do not have permission to perform this action.");
        if (up == null)
        {
            throw new BO.BlDoesNotExistException($"volunteer with id {boVolunteer.Id} does not exist");
        }
        if (!VolunteerManager.CheckValidation(boVolunteer))
            throw new BO.BlDoesNotExistException("The details entered are incorrect.");
        if (boVolunteer.Address != null)
        {
            var (latitude, longitude) = Tools.GetCoordinates(boVolunteer.Address);
            if (latitude != null && longitude != null)
            {
                boVolunteer.Latitude = latitude;
                boVolunteer.Longitude = longitude;
            }
            else
                return;
        }

        try
        {
            DO.Volunteer? prevDoVolunteer = _dal.Volunteer.Read(boVolunteer.Id);
            if (prevDoVolunteer is null)
                throw new BO.BlDoesNotExistException($"volunteer with id {boVolunteer.Id} does not exist");
            if (requester.Role != DO.Role.Manager && (DO.Role)boVolunteer.Role != prevDoVolunteer.Role)
            {
                boVolunteer.Role = (BO.Role)prevDoVolunteer.Role;
            }
            DO.Volunteer doVolunteer =
          new(boVolunteer.Id, boVolunteer.FullName, boVolunteer.Phone, boVolunteer.Email,
          boVolunteer.Password, (DO.TypeDistance)boVolunteer.TypeDistance, (DO.Role)boVolunteer.Role, boVolunteer.Address, boVolunteer.Latitude,
        boVolunteer.Longitude, boVolunteer.IsActive,
        boVolunteer.MaxDistance);

            _dal.Volunteer.Update(doVolunteer);
        }
        catch
        {
            throw new BO.BlDoesNotExistException($"volunteer with id {boVolunteer.Id} does not exist");

        }
    }

    public BO.Volunteer? Read(int id)
    {
        try
        {
            var doVolunteer = _dal.Volunteer.Read(id) ??
                throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");

            var myAssignments = _dal.Assignment.ReadAll(a => a.VolunteerId == doVolunteer.Id);
            List<Assignment>? closedAssignments = myAssignments.Where(a => a.EndOfTreatmentTime != null).ToList();
            int TotalHandledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
            int TotalCanceledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
            int TotalExpiredHandledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

            Assignment? activeAssignment = myAssignments.FirstOrDefault(a => a.TypeOfEnding == null);

            BO.CallInProgress? callInHandling = null;



            if (activeAssignment != null)
            {
                var call = _dal.Call.Read(activeAssignment.CallId) ??
                    throw new BO.BlDoesNotExistException($"Call with ID={activeAssignment.CallId} does Not exist");

                callInHandling = new BO.CallInProgress
                {
                    Id = activeAssignment.Id,
                    CallId = call.Id,
                    TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
                    Description = call.VerbalDescription,
                    FullAddress = call.Address,
                    OpeningTime = call.OpeningTime,
                    MaxCompletionTime = call.MaxTimeFinishRead, // Use the correct property for max completion
                    EntryTimeForHandling = activeAssignment.EntryTimeForTreatment,
                    DistanceFromVolunteer = Tools.DistanceCalculation(call.Address, doVolunteer.Address!),
                    Status = CallManager.GetCallStatus(call.Id)
                };

            }

            //return new BO.Volunteer
            //{
            //    Id = doVolunteer.Id, // Ensure the correct ID is used
            //    FullName = doVolunteer.Name,
            //    IsActive = doVolunteer.IsActive ?? false, // Safely handle potential null
            //    Phone = doVolunteer.Phone,
            //    Email = doVolunteer.Email,
            //    Password = doVolunteer.Password, // Be cautious with sensitive information
            //    TypeDistance = (BO.TypeDistance)doVolunteer.Type_Distance,
            //    Role = (BO.Role)doVolunteer.Role,
            //    Address = doVolunteer.Address,
            //    Latitude = doVolunteer.Latitude,
            //    Longitude = doVolunteer.Longitude,
            //    MaxDistance = doVolunteer.Max_Distance,
            //    TotalHandledCalls = TotalHandledCalls,
            //    TotalCanceledCalls = TotalCanceledCalls,
            //    TotalExpiredHandledCalls = TotalExpiredHandledCalls,
            //    CurrentCallInProgress = callInHandling
            //};



            return new BO.Volunteer(
    doVolunteer.Id, // Ensure the correct ID is used
    doVolunteer.Name,
    doVolunteer.Phone,
    doVolunteer.Email,
    doVolunteer.Password, // Be cautious with sensitive information
    (BO.TypeDistance)doVolunteer.Type_Distance, // Ensure correct casting
    (BO.Role)doVolunteer.Role,
    doVolunteer.Address,
    doVolunteer.Latitude,
    doVolunteer.Longitude,
    doVolunteer.IsActive ?? false, // Safely handle potential null
   (double)doVolunteer.Max_Distance,
    TotalHandledCalls,
    TotalCanceledCalls,
    TotalExpiredHandledCalls,
    callInHandling
    );


        }
        catch (DO.DalDoesNotExistException ex)
        {
            // Log the exception or handle it if necessary
            throw new BO.BlDoesNotExistException($"Unable to find volunteer with ID={id}.", ex);
        }
    }
}







//    public BO.Student? Read(int id)
//    {
//        var doStudent = _dal.Student.Read(id) ??
//throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");
//        return new()
//        {
//            Id = id,
//            Name = doStudent.Name,
//            Alias = doStudent.Alias,
//            IsActive = doStudent.IsActive,
//            BirthDate = doStudent.BirthDate,
//            RegistrationDate = doStudent.RegistrationDate,
//            CurrentYear = StudentManager.GetStudentCurrentYear(doStudent.RegistrationDate)
//        };
//    }


//    //...
//    public void RegisterStudentToCourse(int studentId, int courseId)
//                => LinkManager.LinkStudentToCourse(studentId, courseId);
//    //...
//    public void UpdateGrade(int studentId, int courseId, double grade)
//        => LinkManager.UpdateCourseGradeForStudent(studentId, courseId, grade);
//    //...
//    public BO.StudentGradeSheet GetGradeSheetPerStudent(int studentId, BO.Year year = BO.Year.None)
//    {
//        //...
//    }
//    //...

//}
