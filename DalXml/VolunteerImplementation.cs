

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Volunteer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Volunteer item)
    {
        throw new NotImplementedException();
    }



    int Id,
    string Name,
    string Phone,
    string Email,
    string Password,
    string? Address = null,
    double? Latitude = null,
    double? Longitude = null,
    Role Role= Role.Volunteer,
    bool? IsActive = null,
    double? Max_Distance = null,
    TypeDistance Type_Distance = TypeDistance.Air





    static Student getStudent(XElement s)
    {
        return new DO.Student()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)s.Element("Name") ?? "",
            Alias = (string?)s.Element("Alias") ?? null,
            IsActive = (bool?)s.Element("IsActive") ?? false,
            //CurrentYear = s.ToEnumNullable<Year>("CurrentYear") ?? Year.FirstYear,
            BirthDate = s.ToDateTimeNullable("BirthDate"),
            RegistrationDate = s.ToDateTimeNullable("RegistrationDate")
        };
    }
    public Student? Read(int id)
    {
        XElement? studentElem =
    XMLTools.LoadListFromXMLElement(Config.s_students_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return studentElem is null ? null : getStudent(studentElem);
    }

    public Student? Read(Func<Student, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_students_xml).Elements().Select(s => getStudent(s)).FirstOrDefault(filter);
    }

    public void Update(Student item)
    {
        XElement studentsRootElem = XMLTools.LoadListFromXMLElement(Config.s_students_xml);

        (studentsRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistException($"Student with ID={item.Id} does Not exist"))
                .Remove();

        studentsRootElem.Add(new XElement("Student", createStudentElement(item)));

        XMLTools.SaveListToXMLElement(studentsRootElem, Config.s_students_xml);
    }
 }
