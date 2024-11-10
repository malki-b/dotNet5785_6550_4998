
namespace DO;

public record Call
(
    int Id,
    string Address,
    double Latitude,
    double Longitude,
    DateTime OpeningTime,
     string? VerbalDescription = null,
    DateTime? MaxTimeFinishRead = null,
    TypeOfReading TypeOfReading = TypeOfReading.FoodPreparation
);
