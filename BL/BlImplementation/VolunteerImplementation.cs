namespace BlImplementation;
using BlApi;
using BO;
using Helpers;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Volunteer boVolunteer)
    {
        //ClockManager.Now, 
        var user = _dal.Volunteer.ReadAll().FirstOrDefault(u => u.Name == boVolunteer.FullName);

        DO.Volunteer doVolunteer =
         new(boVolunteer.Id, boVolunteer.FullName, boVolunteer.Phone, boVolunteer.Email,
         boVolunteer.Password, (DO.TypeDistance)boVolunteer.TypeDistance, (DO.Role)boVolunteer.Role, boVolunteer.Address, boVolunteer.Latitude,
       boVolunteer.Longitude, boVolunteer.IsActive,
       boVolunteer.MaxDistance);
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Student with ID={boVolunteer.Id} already exists", ex);
        }
    }


    //public void Create(BO.Volunteer boVolunteer)
    //{
    //    try
    //    {
    //        var user = Volunteer_dal.Volunteer.ReadAll()
    //            .FirstOrDefault(u => u.Name == username);

    //        if (user == null || user.Password != password)
    //            throw new BO.InvalidCredentialsException("שם המשתמש או הסיסמה אינם נכונים.");

    //        return (BO.Role)user.Role;
    //    }
    //    catch (DO.DataAccessException ex)
    //    {
    //        throw new BO.DataAccessException("שגיאה בגישה לנתוני משתמשים.", ex);
    //    }
    //}


    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    //public BO.StudentGradeSheet GetGradeSheetPerStudent(int studentId, BO.Year year = null)
    //{
    //    throw new NotImplementedException();
    //}

    public BO.Role Login(string username, string password)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public IEnumerable<VolunteerInList> ReadAll(BO.VolunteerSortBy? sort = null, VolunteerField? filter = null, object? value = null)
    {
        throw new NotImplementedException();
    }

        public void Update(int requesterId, BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }

    public void Update(Volunteer boStudent)
    {
        throw new NotImplementedException();
    }

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
