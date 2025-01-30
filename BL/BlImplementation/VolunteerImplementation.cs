namespace BlImplementation;
using BlApi;
using BO;
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
            var volunteer = _dal.Volunteer.Read(id)
                ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");

        // Check if the volunteer is handling any cases
        if (!AssignmentManager.VolunteerIsOnCall(id))
        {
            try
            {
                // Attempt to delete the volunteer from the data access layer
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

    public IEnumerable<BO.VolunteerInList> ReadAll(BO.Active? sort = null, BO.VolunteerFields? filter = null, object? value = null)
    {
        //try
        //{
        //    var volunteers = Volunteer_dal.Volunteer.ReadAll();
        //    // סינון לפי סטטוס
        //    if (sort.HasValue)
        //        volunteers = volunteers.Where(v => v.Active == (sort == BO.Active.TRUE)).ToList();
        //    var volunteerList = volunteers.Select(v => new BO.VolunteerInList
        //    {
        //        Id = v.Id,
        //        Name = v.Name,
        //        Active = v.Active
        //    });
        //    // מיון לפי שדה ספציפי
        //    if (filter.HasValue)
        //    {
        //        volunteerList = filter switch
        //        {
        //            BO.VolunteerFields.Name => volunteerList.OrderBy(v => v.Name),
        //            BO.VolunteerFields.Id => volunteerList.OrderBy(v => v.Id),
        //            _ => volunteerList.OrderBy(v => v.Id)
        //        };
        //    }
        //    return volunteerList.ToList();
        //}
        //catch (DO.DataAccessException ex)
        //{
        //    throw new BO.DataAccessException("שגיאה בגישה לנתוני מתנדבים.", ex);
        //}
    }

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
                return _dal.Volunteer.ReadAll(isActive);
            }
            if (sortBy == null)
            {
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

    //public IEnumerable<VolunteerInList> ReadAll(BO.VolunteerSortBy? sort = null, VolunteerField? filter = null, object? value = null)
    //{
    //    throw new NotImplementedException();
    //}

    public void Update(int requesterId, BO.Volunteer volunteer)
    {

    }

    //public void Update(Volunteer boStudent)
    //{
    //    throw new NotImplementedException();
    //}

    BO.Volunteer? IVolunteer.Read(int id)
    {
        var doVolunteer = _dal.Volunteer.Read(id) ??
       throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
        return new()
        {


            Id = id,
            FullName = doVolunteer.Name,
            IsActive = (bool)doVolunteer.IsActive,
            Phone = doVolunteer.Phone,
            Email = doVolunteer.Email,
            Password = doVolunteer.Password,

            TypeDistance = doVolunteer.TypeDistance,
            Role = doVolunteer.Role,
            Address = doVolunteer.Address,
            Latitude = doVolunteer.Latitude,
            Longitude = doVolunteer.Longitude,
            MaxDistance = doVolunteer.MaxDistance,

            CurrentYear = VolunteerManager.GetStudentCurrentYear(doVolunteer.RegistrationDate)

        };
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
