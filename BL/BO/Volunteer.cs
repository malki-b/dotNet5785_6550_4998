
using Helpers;

namespace BO;

public class Student
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public TypeDistance Type_Distance { get; set; }
    public Role Role { get; set; } 
    public string? Address { get; set; } = null;
    public double? Latitude { get; set; } = null;
    public double? Longitude { get; set; } = null;
    public bool? IsActive { get; set; } = null;
    public double? Max_Distance { get; set; } = null;

    public DateTime? BirthDate { get; set; }
    public DateTime RegistrationDate { get; init; }
    public Year CurrentYear { get; init; }

    public override string ToString() => this.ToStringProperty();
}