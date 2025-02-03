namespace DO;

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
    None,
    FearOfHumanLife,
    ImmediateDanger,
    LongTermDanger,
}
/// <summary>
/// for Assignment
/// </summary>
public enum TypeOfEnding//סוג סיום הטיפול
{
    //None,
    Teated,
    ManagerCancellation,
    SelfCancellation,//ביטול עצמי
    CancellationHasExpired//ביטול פג תוקף
}

//public enum CallType
//{
//    FearOfHumanLife,
//    ImmediateDanger,
//    LongTermDanger,
//    //None,
//    //    Regular,
//    //    Emergency,
//    //    HighPriority
//}
