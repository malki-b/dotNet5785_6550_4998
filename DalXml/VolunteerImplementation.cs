

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        XElement volunteerRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

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
        XElement? studentElem =
    XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return studentElem is null ? null : getStudent(studentElem);
    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().Select(s => getStudent(s)).FirstOrDefault(filter);
    }


    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Volunteer item)
    {
        XElement volunteerRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteerRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist"))
                .Remove();

        volunteerRootElem.Add(new XElement(createVolunteerElement(item)));

        XMLTools.SaveListToXMLElement(volunteerRootElem, Config.s_volunteers_xml);
    }







    static Volunteer getStudent(XElement s)
    {
        return new DO.Volunteer()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)s.Element("Name") ?? "",
            Phone = (string?)s.Element("Alias") ?? null,
            IsActive = (bool?)s.Element("IsActive") ?? false,
            //CurrentYear = s.ToEnumNullable<Year>("CurrentYear") ?? Year.FirstYear,
            BirthDate = s.ToDateTimeNullable("BirthDate"),
            RegistrationDate = s.ToDateTimeNullable("RegistrationDate")
        };
    }

    
    XElement createVolunteerElement(Volunteer volunteer)
    {
        return new XElement("Volunteer",
            new XElement("Id", volunteer.Id),
            new XElement("Name", volunteer.Name),
            new XElement("Phone", volunteer.phone),
            new XElement("Alias", volunteer.Alias ?? ""),
            new XElement("IsActive", volunteer.IsActive),
            new XElement("BirthDate", volunteer.BirthDate),
            new XElement("RegistrationDate", volunteer.RegistrationDate));
    }
}

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