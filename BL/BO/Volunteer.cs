
using Helpers;

namespace BO;

public class Volunteer
{
    /// <summary>
    /// Represents the ID of the volunteer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Represents the full name of the volunteer (first and last name).
    /// </summary>
    public string Name { get; set; }

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
    public string? Password { get; set; }

    /// <summary>
    /// Represents the type of distance for the volunteer.
    /// </summary>
    public TypeDistance Type_Distance { get; set; }

    /// <summary>
    /// Represents the role of the volunteer.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Represents the current full address of the volunteer.
    /// </summary>
    public string? Address { get; set; } = null;

    /// <summary>
    /// Represents the latitude of the volunteer's location.
    /// </summary>
    public double? Latitude { get; set; } = null;

    /// <summary>
    /// Represents the longitude of the volunteer's location.
    /// </summary>
    public double? Longitude { get; set; } = null;

    /// <summary>
    /// Indicates whether the volunteer is active.
    /// </summary>
    public bool? IsActive { get; set; } = null;

    /// <summary>
    /// Represents the maximum distance for receiving calls.
    /// </summary>
    public double? Max_Distance { get; set; } = null;

    /// <summary>
    /// Represents the birth date of the volunteer.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Represents the registration date of the volunteer.
    /// </summary>
    public DateTime RegistrationDate { get; init; }

    /// <summary>
    /// Represents the current year for the volunteer.
    /// </summary>
    public Year CurrentYear { get; init; }

    public override string ToString() => this.ToStringProperty();
}