using Helpers;
namespace BO;
public class StudentInCourse
{
    public int StudentId { get; init; }
    public required Tuple<int, string, string> Course
    {
        get; init;

    public Year? InYear { get; init; }
    public SemesterNames? InSemester { get; init; }
    public double? Grade { get; set; }
    public int? Credits { get; init; }
    public override string ToString() => this.ToStringProperty();
}
