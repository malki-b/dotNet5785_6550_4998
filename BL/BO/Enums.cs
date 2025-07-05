namespace BO;



public enum Role
{
    Volunteer,
    Manager,
}
public enum TypeDistance
{
    None,
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
/// <summary>
/// Possible statuses of a call.
/// </summary>
public enum Status
{
    None,
    Open,
    InProgress,
    Closed,
    Expired,
    OpenAtRisk
}

public enum CallStatusEnum
{
    InProgress, // Currently being handled
    AtRisk      // Close to the maximum allowed time
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




/// <summary>
/// Enumeration for call completion types.
/// </summary>
public enum CallCompletionType
{
    Completed,
    Canceled,
    Expired
}
/// <summary>
/// Specifies the fields by which a list of volunteers can be sorted.
/// </summary>

public enum VolunteerSortBy
{

    Id,
    FullName,
    Phone,
    Email,
    Role,
    IsActive,
    MaxDistance,
    TotalHandledCalls,
    TotalCanceledCalls,
    TotalExpiredCalls
}



public enum VolunteerInListFields
{
    None,
    VolunteerId,
    FullName,
    IsActive,
    TotalHandledRequests,
    TotalCanceledRequests,
    TotalExpiredRequests,
    HandledRequestId,
    TypeOfReading
}




public enum CallField
{
    None,
    Id,
    CallType,
    Description,
    FullAddress,
    Latitude,
    Longitude,
    OpeningTime,
    MaxEndTime,
    CallStatus,
}
/// <summary>
/// Enumeration representing the time units for advancing the system clock.
/// </summary>
public enum TimeUnit
{
    Minute,
    Hour,
    Day,
    Month,
    Year
}

public enum VolunteerField
{
    None,
    Id,
    FullName,
    MobilePhone,
    Email,
    Password,
    CurrentFullAddress,
    Latitude,
    Longitude,
    Role,
    IsActive,
    MaxDistanceForCalls,
    DistanceMeasurementType,
    TotalHandledCalls,
    TotalCanceledCalls,
    TotalExpiredHandledCalls,
    CurrentCallInProgress
}
public enum ClosedCallField
{
    None,
    Id,
    TypeOfReading,
    FullAddress,
    OpeningTime,
    EntryTimeForHandling,
    ActualHandlingEndTime,
    TypeOfEnding
}
public enum OpenCallField
{
    None,
    Id,
    Type,
    Description,
    FullAddress,
    OpeningTime,
    MaxCompletionTime,
    DistanceFromVolunteer
}

public enum CallInListFields
{
    None,
    AssignmentId,
    CallId,
    TypeOfReading,
    OpeningTime,
    RemainingTimeToEndCall,
    LastVolunteerName,
    TotalHandlingTime,
    Status,
    TotalAssignments
}


