namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Volunteer boVolunteer)
    {
        try
        {
            //ClockManager.Now, 
            //ClockManager.Now, 
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
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive, BO.VolunteerSortBy? sortBy)
    {
        try
        {
            var Volunteer = _dal.Volunteer.ReadAll();

            if (isActive == null)
            {
                //?? throw new BO.BlDoesNotExistException("Volunteer with ID does Not exist");
                return (IEnumerable<VolunteerInList>)Volunteer;
            }
            else
            {
                var xx = _dal.Volunteer.ReadAll().Where(v => v.IsActive == isActive.Value).ToString();
                return (IEnumerable<BO.VolunteerInList>)xx;

            }
            if (sortBy == null)
            {
                var xx = _dal.Volunteer.ReadAll().Where(v => v.id == isActive.Value).ToString(); //תז

                return _dal.Volunteer.ReadAll(isActive);
            }
            else
            {
                //
            }

            var doVolunteer = _dal.Volunteer.ReadAll(id) ??
                  throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException("Login failed.", ex);
        }
    }


    public IEnumerable<BO.VolunteerInList> ReadAll1(bool? isActive = null, BO.Volunteer_In_List_Fields? sort = null)
    {
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll(isActive is null ? null : v => v.IsActive == isActive);
        var volunteerInList = volunteers.Select(v =>
        {
            int? callId = _dal.Assignment.Read(a => a.VolunteerId == v.Id && a.FinishTime == null)?.CallId;
            BO.CallType? callType = callId is null ? null : (BO.CallType?)_dal.Call.Read(callId.Value)!.CallType;
            return new BO.VolunteerInList(
            v.Id, v.Name, v.IsActive,
            _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id && a.FinishType == DO.Finish_Type.Addressed).Count(),
            _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id && (a.FinishType == DO.Finish_Type.ManageCancel || a.FinishType == DO.Finish_Type.SelfCancel)).Count(),
            _dal.Assignment.ReadAll(a => a.VolunteerId == v.Id && a.FinishType == DO.Finish_Type.Expired).Count(),
            callId, callType);
        }).ToList();

        sort ??= BO.Volunteer_In_List_Fields.Id;

        var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sort.Value.ToString());

        if (propertyInfo != null)
        {
            volunteerInList = volunteerInList.OrderBy(v => propertyInfo.GetValue(v)).ToList();
        }

        return volunteerInList;
    }

    //public IEnumerable<VolunteerInList> ReadAll(BO.VolunteerSortBy? sort = null, VolunteerField? filter = null, object? value = null)
    //{
    //    throw new NotImplementedException();
    //}





    public async void Update(int id, BO.Volunteer boVolunteer)
    {
        DO.Volunteer? requester = _dal.Volunteer.Read(id);
        if (requester is null || (boVolunteer.Id != id && requester.Role != DO.Role.Manager))
            return;
        if (!VolunteerManager.CheckValidation(boVolunteer))
            return;
        if (boVolunteer.Address != null)
        {
            var (latitude, longitude) = await VolunteerManager.GetCoordinatesAsync(boVolunteer.Address);
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
            int TotalCanceledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);
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

            return new BO.Volunteer
            {
                Id = doVolunteer.Id, // Ensure the correct ID is used
                FullName = doVolunteer.Name,
                IsActive = doVolunteer.IsActive ?? false, // Safely handle potential null
                Phone = doVolunteer.Phone,
                Email = doVolunteer.Email,
                Password = doVolunteer.Password, // Be cautious with sensitive information
                TypeDistance = (BO.TypeDistance)doVolunteer.Type_Distance,
                Role = (BO.Role)doVolunteer.Role,
                Address = doVolunteer.Address,
                Latitude = doVolunteer.Latitude,
                Longitude = doVolunteer.Longitude,
                MaxDistance = doVolunteer.Max_Distance,
                TotalHandledCalls = TotalHandledCalls,
                TotalCanceledCalls = TotalCanceledCalls,
                TotalExpiredHandledCalls = TotalExpiredHandledCalls,
                CurrentCallInProgress = callInHandling
            };
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
