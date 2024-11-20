﻿
namespace Dal;

internal static class Config
{
    internal const int startCallId = 1000;
    private static int nextCallId = startCallId;
    internal static int NextCallId { get => nextCallId++; }
    //...
    internal const int startAssignmentId = 1000;
    private static int nextAssignmentId = startAssignmentId;
    internal static int NextAssignmentId { get => nextAssignmentId++; }
    internal static DateTime Clock { get; set; } = DateTime.Now;
    //...

    internal static void Reset()
    {
        nextCallId = startCallId;
        nextAssignmentId = startAssignmentId;
        //...
        Clock = DateTime.Now;
       //internal const int TimeSpan;
    //...
}
}
