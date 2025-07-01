
namespace Dal;

internal static class Config
{
    internal const int startCallId = 1000;
    private static int nextCallId = startCallId;
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static int NextCallId { get => nextCallId++; }

    internal const int startAssignmentId = 1000;

    private static int nextAssignmentId = startAssignmentId;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static int NextAssignmentId { get => nextAssignmentId++; }
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static DateTime Clock { get; set; } = new DateTime(2024, 12, 08, 19, 59, 30);

    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static TimeSpan RiskRange { get; set; } = TimeSpan.FromHours(1);

    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void Reset()
    {
        nextCallId = startCallId;
        nextAssignmentId = startAssignmentId;
        //...
        Clock = new DateTime(2024, 12, 08, 19, 59, 30);
        RiskRange = TimeSpan.FromHours(1);
        //internal const int TimeSpan;
        //...
    }
}