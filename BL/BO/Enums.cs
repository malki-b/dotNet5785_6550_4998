namespace BO;


public enum Role
{
    Volunteer,
    Manager,
}
public enum TypeDistance
{
    Air,
    Walking,
    Drive
}
/// <summary>
/// fot call
/// </summary>
public enum TypeOfReading
{
    FearOfHumanLife,
    ImmediateDanger,
    LongTermDanger,
}
/// <summary>
/// for Assignment
/// </summary>
public enum TypeOfEnding//סוג סיום הטיפול
{
    Teated,
    SelfCancellation,//ביטול עצמי
    CancellationHasExpired//ביטול פג תוקף
}
public enum CallType//סוג קריאה
{

}
public enum Status
{
    Open,
    InProgress,
    Closed,
    Expired,
    OpenAtRisk
}