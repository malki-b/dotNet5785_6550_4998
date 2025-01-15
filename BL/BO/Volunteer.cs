using Helpers;
namespace BO;
public class Student
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Alias { get; set; }
    public bool IsActive { get; set; }
    DateTime? BirthDate { get; set; }
    public DateTime RegistrationDate { get; init; }
    Year CurrentYear { get; init; } //(BO.Year)(ClockManager.Now.Year - registrationDate?.Year!);
    public override string ToString() => this.ToStringProperty();
}

