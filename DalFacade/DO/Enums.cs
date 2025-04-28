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
    None,
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

public enum MainMenu
{
    Exit, DisplayVolunteer, DisplayAssignments, DisplayCalls, DisplayConfig, InitializeData, ResetDatabase, DisplayAllData
}
public enum Crud
{
    Exit, Create, Read, ReadAll, Update, Delete, DeleteAll
}
public enum Config
{
    Exit, AddClockMinute, AddClockHour, AddClockByDay, AddClockByMonth, AddClockByYear, ShowCurrentClock, ChangeClock, ShowCurrentRiskRange, ResetConfig
}