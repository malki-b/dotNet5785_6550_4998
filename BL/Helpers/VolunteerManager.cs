using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

internal static class VolunteerManager
{
    private static IDal s_dal = Factory.Get; //stage 4
    internal static BO.Year GetStudentCurrentYear(DateTime? registrationDate)
    {
        BO.Year currYear = (BO.Year)(ClockManager.Now.Year - registrationDate?.Year!);
        return currYear > BO.Year.None ? BO.Year.None : currYear;
    }
    internal static BO.StudentInCourse GetDetailedCourseForStudent(int studentId, int courseId)
    {
        DO.Link? doLink = s_dal.Link.Read(l => l.StudentId == studentId && l.CourseId == courseId)
            ?? throw new BO.BlDoesNotExistException($"Student with ID={studentId} does Not take Course with ID={courseId}");
        DO.Call? doCall = s_dal.Call.Read(courseId)
     ?? throw new BO.BlDoesNotExistException($"Course with ID={courseId} does Not exist");

        return new()
        {
            StudentId = studentId,
            Course = new Tuple<int, string, string>(doCall.Id, doCall.CourseNumber, doCall.CourseName),
            InYear = (BO.Year?)doCall.InYear,
            InSemester = (BO.SemesterNames?)doCall.InSemester,
            Grade = doLink.Grade,
            Credits = doCall.Credits
        };
    }

    internal static void PeriodicStudentsUpdates(DateTime oldClock, DateTime newClock) //stage 4
    {
        var list = s_dal.Volunteer.ReadAll().ToList();
        foreach (var doStudent in list)
        {
            //if student study for more than MaxRange years
            //then student should be automatically updated to 'not active'
            if (ClockManager.Now.Year - doStudent.RegistrationDate?.Year >= s_dal.Config.MaxRange)
            {
                s_dal.Volunteer.Update(doStudent with { IsActive = false });
            }
        }
    }

}
