namespace BlImplementation;
using BlApi;
using Helpers;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Volunteer boVolunteer)
    {
        DO.Volunteer doVolunteer =
      new(boVolunteer.Id, ClockManager.Now, boVolunteer.FullName, boVolunteer.Alias,
      boVolunteer.IsActive, boVolunteer.Phone, boVolunteer.Email, boVolunteer.Password,
       boVolunteer.TypeDistance, boVolunteer.Role, boVolunteer.Address, boVolunteer.Latitude,
       boVolunteer.Longitude, boVolunteer.MaxDistance);
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.Exceptions($"Student with ID={boVolunteer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.StudentGradeSheet GetGradeSheetPerStudent(int studentId, BO.Year year = null)
    {
        throw new NotImplementedException();
    }

    public BO.Role Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Volunteer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<VolunteerInList> ReadAll(VolunteerSortBy? sort = null, VolunteerField? filter = null, object? value = null)
    {
        throw new NotImplementedException();
    }

    public void RegisterStudentToCourse(int studentId, int courseId)
    {
        throw new NotImplementedException();
    }

    public void RequestDeleteVolunteer(int volunteerId)
    {
        throw new NotImplementedException();
    }

    public BO.Volunteer RequestVolunteerDetails(int volunteerId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.VolunteerInList> RequestVolunteersList(bool? isActive, BO.VolunteerSortBy? sortBy)
    {
        throw new NotImplementedException();
    }

    public void UnRegisterStudentFromCourse(int studentId, int courseId)
    {
        throw new NotImplementedException();
    }

    public void Update(Volunteer boStudent)
    {
        throw new NotImplementedException();
    }

    public void UpdateVolunteerDetails(int requesterId, BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}






    public BO.Student? Read(int id)
    {
        var doStudent = _dal.Student.Read(id) ??
throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");
        return new()
        {
            Id = id,
            Name = doStudent.Name,
            Alias = doStudent.Alias,
            IsActive = doStudent.IsActive,
            BirthDate = doStudent.BirthDate,
            RegistrationDate = doStudent.RegistrationDate,
            CurrentYear = StudentManager.GetStudentCurrentYear(doStudent.RegistrationDate)
        };
    }


    //...
    public void RegisterStudentToCourse(int studentId, int courseId)
                => LinkManager.LinkStudentToCourse(studentId, courseId);
    //...
    public void UpdateGrade(int studentId, int courseId, double grade)
        => LinkManager.UpdateCourseGradeForStudent(studentId, courseId, grade);
    //...
    public BO.StudentGradeSheet GetGradeSheetPerStudent(int studentId, BO.Year year = BO.Year.None)
    {
        //...
    }
    //...

}
