

namespace DO;

public record Assignment
(
    int Id,
    int CallId,
    int VolunteerId,
    DateTime EntryTimeForTreatment,
    DateTime? EndOfTreatmentTime = null,
    TypeOfTreatmentTermination? TypeOfTreatmentTermination = null
);
