using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
namespace BlApi;

public interface IVolunteer
{
  
   void RegisterStudentToCourse(int studentId, int courseId);
    void UnRegisterStudentFromCourse(int studentId, int courseId);

    //IEnumerable<BO.CallInList> GetRegisteredCoursesForStudent(int studentId, BO.Year year = BO.Year.None);
    //IEnumerable<BO.CallInList> GetUnRegisteredCoursesForStudent(int studentId, BO.Year year = BO.Year.None);

    BO.StudentGradeSheet GetGradeSheetPerStudent(int studentId, BO.Year year = BO.Year.None);
    //void UpdateGrade(int studentId, int courseId, double grade);

    /// <summary>
    /// Method to add a new volunteer.
    /// </summary>
    /// <param name="volunteer">The Volunteer object to be added.</param>
    //public void AddVolunteer(BO.Volunteer volunteer);//code
    void Create(BO.Volunteer boStudent);

    /// <summary>
    /// Method to log in to the system.
    /// </summary>
    /// <param name="username">The username of the volunteer.</param>
    /// <param name="password">The password of the volunteer.</param>
    /// <returns>The role of the user.</returns>
    public BO.Role Login(string username, string password);

    /// <summary>
    /// Method to request a list of volunteers.
    /// </summary>
    /// <param name="isActive">Filter for active/inactive volunteers (nullable).</param>
    /// <param name="sortBy">The enumeration field to sort by (nullable).</param>
    //public IEnumerable<BO.VolunteerInList> RequestVolunteersList(bool? isActive, BO.VolunteerSortBy? sortBy);//code
    IEnumerable<BO.VolunteerInList> ReadAll(BO.VolunteerSortBy? sort = null, BO.VolunteerField? filter = null, object? value = null);

    /// <summary>
    /// Method to request volunteer details.
    /// </summary>
    /// <param name="volunteerId">The ID (T.Z) of the volunteer.</param>
    /// <returns>The Volunteer entity including CallInProgress if applicable.</returns>
    //public BO.Volunteer RequestVolunteerDetails(int volunteerId);//code
    BO.Volunteer? Read(int id);

    /// <summary>
    /// Method to update volunteer details.
    /// </summary>
    /// <param name="requesterId">The ID of the requester.</param>
    /// <param name="volunteer">The Volunteer object containing updated details.</param>
    //public void UpdateVolunteerDetails(int requesterId, BO.Volunteer volunteer);//code
    public void Update(BO.Volunteer boStudent);

    /// <summary>
    /// Method to request deletion of a volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID (T.Z) of the volunteer to be deleted.</param>
    public void Delete(int id);

 

}
