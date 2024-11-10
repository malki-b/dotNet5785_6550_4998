
///A volunteer entity

using System.Data;

namespace DO;

public record Volunteer
(
    int Id,
    string Name,
    string Phone,
    string Email,
    string Password,
    string? Address = null,
    double? Latitude = null,
    double? Longitude = null,
    Role Role= Role.Volunteer,
    Boolean? IsActive = null,
    double? Max_Distance = null,
    TypeDistance Type_Distance = TypeDistance.Air

);