

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

//A class that will implement the CRUD methods that can be performed on any volunteer entity by accessing a data collection of type XML file.According to method 2.
internal class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
       
        XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        int? x = Volunteers.ToNullable<int>("Id");
        if (Volunteers.Elements().Any(volunteer => x == item.Id))
            throw new DalAlreadyExistsException($"Create failed. A volunteer's object with ID={item.Id} already exists in the system");
        Volunteers.Add(createVolunteerElement(item));
        XMLTools.SaveListToXMLElement(Volunteers, Config.s_volunteers_xml);

        // XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        //int x =  (XMLTools.ToEnumNullable(Volunteers ,"Id")

        //XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        //int? x = Volunteers.ToEnumNullable<int>("Id");

        //  (Volunteers.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        //?? throw new DO.DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist"))
        //        .Add(createVolunteerElement(item));

    }


    public void Delete(int id)
    {
        XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        (Volunteers.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)
        ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={id} does Not exist"))
                .Remove();
        XMLTools.SaveListToXMLElement(Volunteers, Config.s_volunteers_xml);
    }

    public void DeleteAll()
    {
        XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        Volunteers.RemoveAll();
        XMLTools.SaveListToXMLElement(Volunteers, Config.s_volunteers_xml);
    }

    public Volunteer? Read(int id)
    {
        XElement? studentElem =
       XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return studentElem is null ? null : getVolunteer(studentElem);
    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().Select(s => getVolunteer(s)).FirstOrDefault(filter);
    }

    //public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    //{
    //    XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
    //    //if (filter == null)
    //    //    return Volunteers;
    //    //else
    //    //    return Volunteers.Where(filter);

    //}
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        XElement Volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        

        if (filter == null)
            return Volunteers.Elements("Volunteer")
                                                  .Select(v => getVolunteer(v));
        else
            return Volunteers.Elements("Volunteer")
                                                 .Select(v => getVolunteer(v)).Where(filter);
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



    private XElement createVolunteerElement(Volunteer item)
    {
        XElement volunteerElem = new XElement("Volunteer",
                            new XElement("Id", item.Id),
                            new XElement("Name", item.Name),
                            new XElement("Phone", item.Phone),
                            new XElement("Email", item.Email),
                            new XElement("Password", item.Password),
                              new XElement("Role", item.Role),
                            new XElement("IsActive", item.IsActive ?? null),
                             new XElement("Type_Distance", item.Type_Distance),
                            new XElement("Address", item.Address ?? string.Empty),
                             new XElement("Latitude", item.Latitude ?? null),
                             new XElement("Longitude", item.Longitude ?? null),
                            new XElement("Max_Distance", item.Max_Distance ?? 0));

        return volunteerElem;
    }


    static Volunteer getVolunteer(XElement s)
    {
        return new DO.Volunteer()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)s.Element("Name") ?? "",
            Phone = (string?)s.Element("Phone") ?? "",
            IsActive = (bool?)s.Element("IsActive") ?? false,
            Address = (string?)s.Element("Address") ?? "",
            Latitude = (double?)s.Element("Latitude") ?? 0,
            Longitude = (double?)s.Element("Longitude") ?? 0,
            Role = Role.Volunteer,
            Max_Distance = (double?)s.Element("Max_Distance") ?? 0,
            Type_Distance = TypeDistance.Air
        };
    }


}
