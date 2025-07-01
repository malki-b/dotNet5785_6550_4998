namespace BlImplementation;
using BlApi;
using BO;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void Create(BO.Volunteer boVolunteer)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            (double Latitude, double Longitude) = Tools.GetCoordinates(boVolunteer.Address);
            DO.Volunteer doVolunteer =
                new(boVolunteer.Id, boVolunteer.FullName, boVolunteer.Phone, boVolunteer.Email,
                    boVolunteer.Password, (DO.TypeDistance)boVolunteer.TypeDistance, (DO.Role)boVolunteer.Role, boVolunteer.Address, Latitude,
                    Longitude, boVolunteer.IsActive, boVolunteer.MaxDistance);
            lock (AdminManager.BlMutex)
            {
                _dal.Volunteer.Create(doVolunteer);
            }
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            bool exists;
            lock (AdminManager.BlMutex)
            {
                exists = _dal.Volunteer.Read(id) != null;
            }
            if (!exists)
                throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");

            if (!AssignmentManager.VolunteerIsOnCall(id))
            {
                try
                {
                    lock (AdminManager.BlMutex)
                    {
                        _dal.Volunteer.Delete(id);
                    }
                    VolunteerManager.Observers.NotifyListUpdated();
                }
                catch (DO.DalNotFoundException ex)
                {
                    throw new BO.BlDoesNotExistException($"Failed to delete volunteer with ID={id}.", ex);
                }
            }
            else
            {
                throw new BO.BlCannotDeleteException("Volunteer cannot be deleted because they are currently handling cases.");
            }
        }
        catch (DO.DalDoNotSuccseedDelete ex)
        {
            throw new BO.BlCannotDeleteException("Volunteer with ID already exists", ex);
        }
    }

    public BO.Role Login(int id, string password)
    {
        try
        {
            DO.Volunteer? user;
            lock (AdminManager.BlMutex)
            {
                user = _dal.Volunteer.ReadAll().FirstOrDefault(u => u.Id == id);
            }
            if (user == null || user.Password != password)
                throw new("The Id or password is incorrect.");
            return (BO.Role)user.Role;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException("Login failed.", ex);
        }
    }

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive = null, BO.VolunteerSortBy? sortBy = null)
    {
        List<DO.Volunteer> volunteers;
        lock (AdminManager.BlMutex)
        {
            volunteers = _dal.Volunteer.ReadAll(isActive is null ? null : v => v.IsActive == isActive).ToList();
        }

        IEnumerable<BO.VolunteerInList> volunteersList = volunteers.Select(volunteer =>
        {
            List<Assignment> volunteerAssignments;
            lock (AdminManager.BlMutex)
            {
                volunteerAssignments = _dal.Assignment.ReadAll(a => a.VolunteerId == volunteer.Id).ToList();
            }

            int TotalHandledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
            int TotalCanceledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
            int TotalExpiredRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

            var currentCallId = volunteerAssignments.FirstOrDefault(a => a.TypeOfEnding == null)?.CallId;

            BO.TypeOfReading callType = BO.TypeOfReading.None;
            if (currentCallId.HasValue)
            {
                lock (AdminManager.BlMutex)
                {
                    var call = _dal.Call.Read(currentCallId.Value);
                    if (call != null)
                        callType = (BO.TypeOfReading)call.TypeOfReading;
                }
            }

            return new BO.VolunteerInList
            {
                VolunteerId = volunteer.Id,
                FullName = volunteer.Name,
                IsActive = volunteer.IsActive ?? false,
                TotalHandledRequests = TotalHandledRequests,
                TotalCanceledRequests = TotalCanceledRequests,
                TotalExpiredRequests = TotalExpiredRequests,
                HandledRequestId = currentCallId,
                TypeOfReading = callType
            };
        });

        if (sortBy == null)
        {
            return volunteersList.OrderBy(v => v.VolunteerId).ToList();
        }

        var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sortBy.ToString());
        if (propertyInfo != null)
        {
            return volunteersList.OrderBy(v => propertyInfo.GetValue(v, null)).ToList();
        }
        return volunteersList;
    }

    public void Update(int requesterId, BO.Volunteer boVolunteer)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        DO.Volunteer? requester;
        DO.Volunteer? up;
        lock (AdminManager.BlMutex)
        {
            requester = _dal.Volunteer.Read(requesterId);
            up = _dal.Volunteer.Read(boVolunteer.Id);
        }
        if (requester is null)
            throw new BO.BlDoesNotExistException("You do not have permission to perform this action.");
        if (up == null)
            throw new BO.BlDoesNotExistException($"volunteer with id {boVolunteer.Id} does not exist");
        if (!VolunteerManager.CheckValidation(boVolunteer))
            throw new BO.BlDoesNotExistException("The details entered are incorrect.");
        if (boVolunteer.Address != null)
        {
            var (latitude, longitude) = Tools.GetCoordinates(boVolunteer.Address);
            if (latitude != null && longitude != null)
            {
                boVolunteer.Latitude = latitude;
                boVolunteer.Longitude = longitude;
            }
            else
                return;
        }

        try
        {
            DO.Volunteer? prevDoVolunteer;
            lock (AdminManager.BlMutex)
            {
                prevDoVolunteer = _dal.Volunteer.Read(boVolunteer.Id);
            }
            if (prevDoVolunteer is null)
                throw new BO.BlDoesNotExistException($"volunteer with id {boVolunteer.Id} does not exist");
            if (requester.Role != DO.Role.Manager && (DO.Role)boVolunteer.Role != prevDoVolunteer.Role)
            {
                boVolunteer.Role = (BO.Role)prevDoVolunteer.Role;
            }
            DO.Volunteer doVolunteer =
                new(boVolunteer.Id, boVolunteer.FullName, boVolunteer.Phone, boVolunteer.Email,
                    boVolunteer.Password, (DO.TypeDistance)boVolunteer.TypeDistance, (DO.Role)boVolunteer.Role, boVolunteer.Address, boVolunteer.Latitude,
                    boVolunteer.Longitude, boVolunteer.IsActive, boVolunteer.MaxDistance);

            lock (AdminManager.BlMutex)
            {
                _dal.Volunteer.Update(doVolunteer);
            }
            VolunteerManager.Observers.NotifyItemUpdated(doVolunteer.Id);
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch
        {
            throw new BO.BlDoesNotExistException($"volunteer with id {boVolunteer.Id} does not exist");
        }
    }

    public BO.Volunteer? Read(int id)
    {
        try
        {
            DO.Volunteer doVolunteer;
            lock (AdminManager.BlMutex)
            {
                doVolunteer = _dal.Volunteer.Read(id) ??
                    throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");
            }

            List<Assignment> myAssignments;
            lock (AdminManager.BlMutex)
            {
                myAssignments = _dal.Assignment.ReadAll(a => a.VolunteerId == doVolunteer.Id).ToList();
            }
            List<Assignment>? closedAssignments = myAssignments.Where(a => a.EndOfTreatmentTime != null).ToList();
            int TotalHandledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
            int TotalCanceledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
            int TotalExpiredHandledCalls = closedAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

            Assignment? activeAssignment = myAssignments.FirstOrDefault(a => a.TypeOfEnding == DO.TypeOfEnding.None);
            BO.CallInProgress? callInHandling = null;

            if (activeAssignment != null)
            {
                DO.Call call;
                lock (AdminManager.BlMutex)
                {
                    call = _dal.Call.Read(activeAssignment.CallId) ??
                        throw new BO.BlDoesNotExistException($"Call with ID={activeAssignment.CallId} does Not exist");
                }
                callInHandling = new BO.CallInProgress
                {
                    Id = activeAssignment.Id,
                    CallId = call.Id,
                    TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
                    Description = call.VerbalDescription,
                    FullAddress = call.Address,
                    OpeningTime = call.OpeningTime,
                    MaxCompletionTime = call.MaxTimeFinishRead,
                    EntryTimeForHandling = activeAssignment.EntryTimeForTreatment,
                    DistanceFromVolunteer = Tools.DistanceCalculation(call.Address, doVolunteer.Address!),
                    Status = CallManager.GetCallStatus(call.Id)
                };
            }

            return new BO.Volunteer(
                doVolunteer.Id,
                doVolunteer.Name,
                doVolunteer.Phone,
                doVolunteer.Email,
                doVolunteer.Password,
                (BO.TypeDistance)doVolunteer.Type_Distance,
                (BO.Role)doVolunteer.Role,
                doVolunteer.Address,
                doVolunteer.Latitude,
                doVolunteer.Longitude,
                doVolunteer.IsActive ?? false,
                (double)doVolunteer.Max_Distance,
                TotalHandledCalls,
                TotalCanceledCalls,
                TotalExpiredHandledCalls,
                callInHandling
            );
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Unable to find volunteer with ID={id}.", ex);
        }
    }

    #region Stage 5
    public void AddObserver(Action listObserver) =>
        VolunteerManager.Observers.AddListObserver(listObserver);
    public void AddObserver(int id, Action observer) =>
        VolunteerManager.Observers.AddObserver(id, observer);
    public void RemoveObserver(Action listObserver) =>
        VolunteerManager.Observers.RemoveListObserver(listObserver);
    public void RemoveObserver(int id, Action observer) =>
        VolunteerManager.Observers.RemoveObserver(id, observer);
    #endregion Stage 5

    public IEnumerable<BO.VolunteerInList> GetFilteredAndSortedVolunteers(
        BO.VolunteerInListFields? filterBy = null,
        object? filterValue = null,
        BO.VolunteerInListFields? sortBy = null)
    {
        List<DO.Volunteer> allVolunteers;
        lock (AdminManager.BlMutex)
        {
            allVolunteers = _dal.Volunteer.ReadAll().ToList();
        }

        IEnumerable<BO.VolunteerInList> volunteersList = allVolunteers.Select(volunteer =>
        {
            List<Assignment> volunteerAssignments;
            lock (AdminManager.BlMutex)
            {
                volunteerAssignments = _dal.Assignment.ReadAll(a => a.VolunteerId == volunteer.Id).ToList();
            }

            int TotalHandledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
            int TotalCanceledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
            int TotalExpiredRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

            var currentCallHandled = volunteerAssignments.FirstOrDefault(a => a.TypeOfEnding == null);
            var currentCallId = currentCallHandled?.CallId;
            BO.TypeOfReading callType = BO.TypeOfReading.None;
            if (currentCallId.HasValue)
            {
                lock (AdminManager.BlMutex)
                {
                    var call = _dal.Call.Read(currentCallId.Value);
                    if (call != null)
                        callType = (BO.TypeOfReading)call.TypeOfReading;
                }
            }

            return new BO.VolunteerInList
            {
                VolunteerId = volunteer.Id,
                FullName = volunteer.Name,
                IsActive = volunteer.IsActive ?? false,
                TotalHandledRequests = TotalHandledRequests,
                TotalCanceledRequests = TotalCanceledRequests,
                TotalExpiredRequests = TotalExpiredRequests,
                HandledRequestId = currentCallId,
                TypeOfReading = callType
            };
        });

        // Filtering
        if (filterBy != null && filterValue != null)
        {
            volunteersList = filterBy switch
            {
                BO.VolunteerInListFields.VolunteerId => volunteersList.Where(v => v.VolunteerId.Equals(Convert.ToInt32(filterValue))),
                BO.VolunteerInListFields.FullName => volunteersList.Where(v => v.FullName.Equals(filterValue.ToString())),
                BO.VolunteerInListFields.IsActive => volunteersList.Where(v => v.IsActive.Equals(Convert.ToBoolean(filterValue))),
                BO.VolunteerInListFields.TotalHandledRequests => volunteersList.Where(v => v.TotalHandledRequests.Equals(Convert.ToInt32(filterValue))),
                BO.VolunteerInListFields.TotalCanceledRequests => volunteersList.Where(v => v.TotalCanceledRequests.Equals(Convert.ToInt32(filterValue))),
                BO.VolunteerInListFields.TotalExpiredRequests => volunteersList.Where(v => v.TotalExpiredRequests.Equals(Convert.ToInt32(filterValue))),
                BO.VolunteerInListFields.HandledRequestId => volunteersList.Where(v => v.HandledRequestId.Equals(Convert.ToInt32(filterValue))),
                BO.VolunteerInListFields.TypeOfReading => volunteersList.Where(v => v.TypeOfReading.Equals((BO.TypeOfReading)filterValue)),
                _ => volunteersList
            };
        }

        // Sorting
        volunteersList = sortBy switch
        {
            BO.VolunteerInListFields.VolunteerId => volunteersList.OrderBy(v => v.VolunteerId),
            BO.VolunteerInListFields.FullName => volunteersList.OrderBy(v => v.FullName),
            BO.VolunteerInListFields.IsActive => volunteersList.OrderBy(v => v.IsActive),
            BO.VolunteerInListFields.TotalHandledRequests => volunteersList.OrderBy(v => v.TotalHandledRequests),
            BO.VolunteerInListFields.TotalCanceledRequests => volunteersList.OrderBy(v => v.TotalCanceledRequests),
            BO.VolunteerInListFields.TotalExpiredRequests => volunteersList.OrderBy(v => v.TotalExpiredRequests),
            BO.VolunteerInListFields.HandledRequestId => volunteersList.OrderBy(v => v.HandledRequestId),
            BO.VolunteerInListFields.TypeOfReading => volunteersList.OrderBy(v => v.TypeOfReading),
            _ => volunteersList.OrderBy(v => v.VolunteerId)
        };

        return volunteersList;
    }
}
