using System.Data;

namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id">Personal unique ID of the volunteer</param>
/// <param name="Name">Private Name of the volunteer</param>
/// <param name="Phone">phone of the volunteer</param>
/// <param name="Email">email of the volunteer</param>
/// <param name="Password">password of the volunteer</param>
/// <param name="Address">address of the volunteer(default empty)</param>
/// <param name="Latitude">a number indicating how far a point on Earth is south or north of the equator(default empty)</param>
/// <param name="Longitude"> a number indicating how far a point on Earth is east or west of the equator(default empty)</param>
/// <param name="Role">"Manager" or "Volunteer"(default empty)</param>
/// <param name="IsActive">Is the volunteer active or inactive?(default empty)</param>
/// <param name="Max_Distance">A volunteer will define through the display the maximum distance to receive a call(default empty)</param>
/// <param name="Type_Distance">Aerial distance, walking distance, driving distance</param>
/// 


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
    bool? IsActive = null,
    double? Max_Distance = null,
    TypeDistance Type_Distance = TypeDistance.Air
);