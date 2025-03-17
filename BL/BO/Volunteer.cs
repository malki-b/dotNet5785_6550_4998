
using Helpers;

namespace BO;


public class Volunteer
{
    private string name;

    //public Volunteer(int id, string name, string phone, string email, string password, TypeDistance typeDistance, Role role, string address, double? latitude, double? longitude, bool isActive, double maxDistance)
    //{
    //    Id = id;
    //    this.name = name;
    //    Phone = phone;
    //    Email = email;
    //    Password = password;
    //    TypeDistance = typeDistance;
    //    Role = role;
    //    Address = address;
    //    Latitude = latitude;
    //    Longitude = longitude;
    //    IsActive = isActive;
    //    MaxDistance = maxDistance;
    //}

    public Volunteer(
    int id,
    string name,
    string phone,
    string email,
    string password,
    TypeDistance typeDistance,
    Role role,
    string address,
    double? latitude,
    double? longitude,
    bool isActive,
    double maxDistance,
    int totalHandledCalls,
    int totalCanceledCalls,
    int totalExpiredHandledCalls,
    CallInProgress? currentCallInProgress)
    {
        Id = id;
        this.name = name;
        Phone = phone;
        Email = email;
        Password = password;
        TypeDistance = typeDistance;
        Role = role;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        IsActive = isActive;
        MaxDistance = maxDistance;
        TotalHandledCalls = totalHandledCalls;
        TotalCanceledCalls = totalCanceledCalls;
        TotalExpiredHandledCalls = totalExpiredHandledCalls;
        CurrentCallInProgress = currentCallInProgress;
    }
    //private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Represents the ID of the volunteer.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Represents the full name of the volunteer (first and last name).
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Represents the mobile phone number of the volunteer.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Represents the email address of the volunteer.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Represents the password of the volunteer.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Represents the current full address of the volunteer.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Represents the latitude of the volunteer's location.
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Represents the longitude of the volunteer's location.
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Represents the role of the volunteer.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Indicates whether the volunteer is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Represents the maximum distance within which the volunteer is willing to receive calls.
    /// </summary>
    public double? MaxDistance { get; set; }

    /// <summary>
    /// Represents the type of distance measurement used by the volunteer.
    /// </summary>
    public TypeDistance TypeDistance { get; set; }

    /// <summary>
    /// Represents the total number of calls handled by the volunteer.
    /// </summary>
    public int TotalHandledCalls { get; set; }

    /// <summary>
    /// Represents the total number of calls canceled by the volunteer.
    /// </summary>
    public int TotalCanceledCalls { get; set; }

    /// <summary>
    /// Represents the total number of calls chosen to handle that expired.
    /// </summary>
    public int TotalExpiredHandledCalls { get; set; }

    /// <summary>
    /// Represents the call currently being handled by the volunteer.
    /// </summary>
    public CallInProgress? CurrentCallInProgress { get; set; }

    public override string ToString() => this.ToStringProperty();
}