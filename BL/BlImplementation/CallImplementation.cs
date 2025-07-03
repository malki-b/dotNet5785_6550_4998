namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using ICall = BlApi.ICall;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Call boCall)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            CallManager.ValidateCall(boCall);
            DO.Call newCallDO = CallManager.ConvertToDO(boCall);
            lock (AdminManager.BlMutex)
            {
                _dal.Call.Create(newCallDO);
            }
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with ID={boCall.CallId} already exists", ex);
        }
    }

    public void Delete(int callId)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            DO.Call? doCall;
            lock (AdminManager.BlMutex)
            {
                doCall = _dal.Call.Read(callId);
            }
            if (doCall == null)
                throw new BO.BlDoesNotExistException($"Call with ID={callId} does not exist.");

            if ((CallManager.GetCallStatus(callId) == BO.Status.Open))
            {
                try
                {
                    lock (AdminManager.BlMutex)
                    {
                        _dal.Call.Delete(callId);
                        DO.Assignment assignment = _dal.Assignment.Read(a => a.CallId == callId)
                            ?? throw new Exception("The requested assignment does not exist");
                        var updatedAssignment = assignment with
                        {
                            TypeOfEnding = DO.TypeOfEnding.ManagerCancellation
                        };
                        _dal.Assignment.Update(updatedAssignment);
                        CallManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
                        CallManager.Observers.NotifyListUpdated();
                        VolunteerManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
                        VolunteerManager.Observers.NotifyListUpdated();
                    }
                }
                catch (DO.DalNotFoundException ex)
                {
                    throw new BO.BlDoesNotExistException($"Failed to delete volunteer with ID={callId}.", ex);
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

    public int[] RequestCallCounts()
    {
        List<DO.Call> calls;
        lock (AdminManager.BlMutex)
        {
            calls = _dal.Call.ReadAll().ToList();
        }
        var statusId = from dalCall in calls
                       select new { Id = dalCall.Id, Status = CallManager.GetCallStatus(dalCall.Id) };

        var counter = from callLine in statusId
                      group callLine by callLine.Status into statusGroup
                      select statusGroup.Count();

        Console.WriteLine(counter.ToArray());
        return counter.ToArray();
    }

    public BO.Call RequestCallDetails(int callId)
    {
        try
        {
            DO.Call? callData;
            lock (AdminManager.BlMutex)
            {
                callData = _dal.Call.Read(c => c.Id == callId);
            }
            if (callData == null)
                throw new BO.BlDoesNotExistException("Call not found");

            List<DO.Assignment> assignmentData;
            lock (AdminManager.BlMutex)
            {
                assignmentData = _dal.Assignment.ReadAll().Where(a => a.CallId == callId).ToList();
            }

            List<BO.CallAssignInList> assignmentList = assignmentData.Select(assignment =>
            {
                string? volunteerName;
                lock (AdminManager.BlMutex)
                {
                    volunteerName = _dal.Volunteer.Read(assignment.VolunteerId)?.Name;
                }
                return new BO.CallAssignInList
                {
                    VolunteerId = assignment.VolunteerId,
                    VolunteerName = volunteerName,
                    EntryTimeForTreatment = assignment.EntryTimeForTreatment,
                    EndOfTreatmentTime = assignment.EndOfTreatmentTime,
                    TypeOfEnding = (BO.TypeOfEnding)assignment.TypeOfEnding,
                };
            }).ToList();

            BO.Call callBO = CallManager.ConvertToBO(callData, assignmentList);
            return callBO;
        }
        catch (Exception ex)
        {
            throw new BO.BlDoesNotExistException($"in RequestCallDetails", ex);
        }
    }

    public IEnumerable<CallInList> ReadAll(BO.CallField? filterField = null, object? filterValue = null, BO.CallField? sortField = null)
    {
        try
        {
            List<DO.Call> calls;
            lock (AdminManager.BlMutex)
            {
                calls = _dal.Call.ReadAll().ToList();
            }
            var callInLists = calls.Select(c =>
            {
                List<DO.Assignment> assignments;
                lock (AdminManager.BlMutex)
                {
                    assignments = _dal.Assignment.ReadAll(a => a.CallId == c.Id).ToList();
                }
                var lastAssignment = assignments.OrderByDescending(a => a.EntryTimeForTreatment).FirstOrDefault();
                string? lastVolunteerName = null;
                if (lastAssignment != null)
                {
                    lock (AdminManager.BlMutex)
                    {
                        lastVolunteerName = _dal.Volunteer.Read(lastAssignment.VolunteerId)?.Name;
                    }
                }
                return new CallInList
                {
                    AssignmentId = lastAssignment?.Id,
                    CallId = c.Id,
                    TypeOfReading = (BO.TypeOfReading)c.TypeOfReading,
                    OpeningTime = c.OpeningTime,
                    RemainingTimeToEndCall = CallManager.getRemainingTimeToEndCall(c),
                    LastVolunteerName = lastVolunteerName,
                    TotalHandlingTime = CallManager.getMaxTimeFinishRead(c),
                    Status = CallManager.GetCallStatus(c.Id),
                    TotalAssignments = assignments.Count()
                };
            });

            if (filterField.HasValue && filterValue != null)
            {
                var prop = typeof(BO.CallInList).GetProperty(filterField.ToString());
                if (prop != null)
                {
                    if (prop.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.Parse(prop.PropertyType, filterValue.ToString());
                        callInLists = callInLists.Where(c => prop.GetValue(c)?.Equals(enumValue) == true);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        callInLists = callInLists.Where(c => prop.GetValue(c)?.Equals(convertedValue) == true);
                    }
                }
            }

            return sortField.HasValue
                ? callInLists.OrderBy(c => typeof(BO.CallInList).GetProperty(sortField.ToString())?.GetValue(c))
                : callInLists.OrderBy(c => c.CallId);
        }
        catch (Exception ex)
        {
            throw new BO.BlCannotDeleteException("Failed to retrieve calls list", ex);
        }
    }

    public IEnumerable<BO.ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, BO.TypeOfReading? filterBy = null, ClosedCallField? sortByField = null)
    {
        try
        {
            DO.Volunteer? volunteer;
            lock (AdminManager.BlMutex)
            {
                volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId);
            }
            if (volunteer == null)
                throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

            List<DO.Assignment> assignments;
            lock (AdminManager.BlMutex)
            {
                assignments = _dal.Assignment.ReadAll().Where(a => a.VolunteerId == volunteerId &&
                            (CallManager.GetCallStatus(a.CallId) == Status.Closed || a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation)).ToList();
            }

            List<DO.Call> calls;
            lock (AdminManager.BlMutex)
            {
                calls = _dal.Call.ReadAll().ToList();
            }

            var joined = from assign in assignments
                         join call in calls on assign.CallId equals call.Id
                         select new { call, assign };

            if (filterBy != null)
                joined = joined.Where(x => x.call.TypeOfReading == (DO.TypeOfReading)filterBy);

            var result = joined.Select(c => new BO.ClosedCallInList
            {
                Id = c.call.Id,
                TypeOfReading = (BO.TypeOfReading)c.call.TypeOfReading,
                FullAddress = c.call.Address,
                OpeningTime = c.call.OpeningTime,
                EntryTimeForHandling = c.assign.EntryTimeForTreatment,
                ActualHandlingEndTime = c.assign.EndOfTreatmentTime,
                TypeOfEnding = (BO.TypeOfEnding)c.assign.TypeOfEnding
            });

            if (sortByField != null)
            {
                var property = typeof(BO.ClosedCallInList).GetProperty(sortByField.ToString());
                if (property != null)
                    result = result.OrderBy(call => property.GetValue(call));
            }
            else
            {
                result = result.OrderBy(call => call.OpeningTime);
            }

            return result;
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BO.BlDoesNotExistException("Error retrieving calls.");
        }
    }

    public IEnumerable<BO.OpenCallInList> RequestOpenCallsForSelection(int volunteerId, BO.TypeOfReading? filterBy = null, OpenCallField? sortByField = null)
    {
        DO.Volunteer? volunteer;
        lock (AdminManager.BlMutex)
        {
            volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId);
        }
        if (volunteer == null)
            throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

        List<DO.Call> calls;
        lock (AdminManager.BlMutex)
        {
            calls = _dal.Call.ReadAll().ToList();
        }
        var openCalls = calls.Where(c =>
        {
            var status = CallManager.GetCallStatus(c.Id);
            return status == Status.Open || status == Status.OpenAtRisk;
        });

        if (filterBy != null)
            openCalls = openCalls.Where(c => c.TypeOfReading == (DO.TypeOfReading)filterBy);

        var openCallList = openCalls.Select(c => new BO.OpenCallInList
        {
            Id = c.Id,
            Type = (BO.TypeOfReading)c.TypeOfReading,
            Description = c.VerbalDescription,
            FullAddress = c.Address,
            OpeningTime = c.OpeningTime,
            MaxCompletionTime = c.MaxTimeFinishRead,
            DistanceFromVolunteer = CallManager.CalculateDistance(volunteer.Latitude, volunteer.Longitude, c.Latitude, c.Longitude)
        });

        if (sortByField != null)
        {
            var prop = typeof(BO.OpenCallInList).GetProperty(sortByField.ToString());
            if (prop != null)
                openCallList = openCallList.OrderBy(c => prop.GetValue(c));
        }
       

        return openCallList;
    }

    public void SelectCallForTreatment(int volunteerId, int callId)
    {
        try
        {
            DO.Call? call;
            lock (AdminManager.BlMutex)
            {
                call = _dal.Call.Read(callId);

            }
            if (call == null)
                throw new BO.BlInvalidException($"Call with ID {callId} not found.");
            var status = CallManager.GetCallStatus(callId);

            bool assignmentExists;
            lock (AdminManager.BlMutex)
            {
                assignmentExists = _dal.Assignment.Read(callId) != null;

            }
            if (status == Status.Expired || status == Status.Closed || (status == Status.InProgress && assignmentExists))
            {
                throw new BO.BlInvalidException($"Cannot select this call for treatment, since the call's status is: {status}");
            }

            var newAssignment = new DO.Assignment(
                CallId: callId,
                VolunteerId: volunteerId,
                EntryTimeForTreatment: AdminManager.Now,
                EndOfTreatmentTime: null,
                TypeOfEnding: DO.TypeOfEnding.None
            );

            lock (AdminManager.BlMutex)
            {
                _dal.Assignment.Create(newAssignment);
            }
            //BO.Call? callBo;
            //lock (AdminManager.BlMutex)
            //{
            //    callBo = RequestCallDetails(callId);

            //    // Update the status to InProgress only in the BO.Call object
            //    if (callBo != null)
            //    {
            //        callBo.CallStatus = Status.InProgress;
            //        _dal.Call.Update(CallManager.ConvertToDO(callBo));
            //    }
                    
            //    // No need to update DAL if you only want to change the BO object in memory
            //}
            //CallManager.Observers.NotifyItemUpdated(callId);
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (BO.BlInvalidException ex)
        {
            throw new BO.BlInvalidException($"Invalid operation: {ex.Message}", ex);
        }
    }

    public void UpdateCallCancellation(int requesterId, int assignmentId)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            DO.Assignment assignment;
            lock (AdminManager.BlMutex)
            {
                assignment = _dal.Assignment.Read(a => a.Id == assignmentId)
                    ?? throw new Exception("The requested assignment does not exist");
            }
            DO.Volunteer volunteer;
            lock (AdminManager.BlMutex)
            {
                volunteer = _dal.Volunteer.Read(v => v.Id == assignment.VolunteerId)
                    ?? throw new Exception("The volunteer was not found in the system");
            }
            bool isAdmin = volunteer.Role == DO.Role.Manager;
            bool isVolunteer = assignment.VolunteerId == requesterId;

            if (!isAdmin && !isVolunteer)
                throw new Exception("You do not have permission to cancel this call");

            if (assignment.EndOfTreatmentTime != null)
                throw new Exception("Cannot cancel a call that has already been closed");

            var updatedAssignment = assignment with
            {
                EndOfTreatmentTime = DateTime.Now,
                TypeOfEnding = isVolunteer ? DO.TypeOfEnding.SelfCancellation : DO.TypeOfEnding.ManagerCancellation
            };
            lock (AdminManager.BlMutex)
            {
                _dal.Assignment.Update(updatedAssignment);
            }
            CallManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while canceling the call", ex);
        }
    }

    public void UpdateCallCompletion(int volunteerId, int assignmentId)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            DO.Assignment assignment;
            lock (AdminManager.BlMutex)
            {
                assignment = _dal.Assignment.Read(a => a.Id == assignmentId)
                    ?? throw new Exception($"Assignment with ID {assignmentId} not found");
            }
            if (assignment.VolunteerId != volunteerId)
                throw new Exception("Volunteer is not authorized to close this call");

            var updatedAssignment = assignment with
            {
                EndOfTreatmentTime = DateTime.Now,
                TypeOfEnding = DO.TypeOfEnding.Teated
            };
            lock (AdminManager.BlMutex)
            {
                _dal.Assignment.Update(updatedAssignment);
            }
            
            CallManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (Exception ex)
        {
            throw new Exception("Error closing call", ex);
        }
    }

    public void UpdateCallDetails(BO.Call call)
    {
        if (call == null)
            throw new BO.BlInvalidException("Call object cannot be null.");

        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
            CallManager.ValidateCall(call);
            var (latitude, longitude) = Tools.GetCoordinates(call.Address);
            if (latitude == 0 || longitude == 0)
                throw new BO.BlInvalidException("Invalid address: Unable to retrieve coordinates.");

            call.Latitude = latitude;
            call.Longitude = longitude;

            DO.Call doCall = CallManager.ConvertToDO(call);
            lock (AdminManager.BlMutex)
            {
                _dal.Call.Update(doCall);
            }
            CallManager.Observers.NotifyItemUpdated(doCall.Id);
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (Exception ex)
        {
            throw new BO.BlInvalidException("Volunteer with ID already exists", ex);
        }
    }

    public IEnumerable<BO.CallInList> GetFilteredAndSortedCalls(
        BO.CallInListFields? filterBy = null, object? filterValue = null,
        BO.CallInListFields? sortBy = null)
    {
        List<DO.Call> allCalls;
        List<DO.Assignment> allAssignments;
        Dictionary<int, string> allVolunteers;
        lock (AdminManager.BlMutex)
        {
            allCalls = _dal.Call.ReadAll().ToList();
            allAssignments = _dal.Assignment.ReadAll().ToList();
            allVolunteers = _dal.Volunteer.ReadAll().ToDictionary(v => v.Id, v => v.Name);
        }

        var list = allCalls.Select(call =>
        {
            var latest = allAssignments.Where(a => a.CallId == call.Id)
                                       .OrderByDescending(a => a.EntryTimeForTreatment).FirstOrDefault();

            return new BO.CallInList
            {
                AssignmentId = latest?.Id,
                CallId = call.Id,
                TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
                OpeningTime = call.OpeningTime,
                RemainingTimeToEndCall = call.MaxTimeFinishRead.HasValue
                    ? (call.MaxTimeFinishRead > DateTime.Now ? call.MaxTimeFinishRead.Value - DateTime.Now : TimeSpan.Zero)
                    : null,
                LastVolunteerName = latest != null && allVolunteers.TryGetValue(latest.VolunteerId, out var name) ? name : null,
                TotalHandlingTime = latest?.EndOfTreatmentTime.HasValue == true
                    ? latest.EndOfTreatmentTime.Value - call.OpeningTime : null,
                Status = CallManager.GetCallStatus(call.Id),
                TotalAssignments = allAssignments.Count(a => a.CallId == call.Id)
            };
        });

        if (filterBy != null && filterValue != null)
        {
            list = filterBy switch
            {
                BO.CallInListFields.AssignmentId => list.Where(c => c.AssignmentId.Equals(Convert.ToInt32(filterValue))),
                BO.CallInListFields.TypeOfReading => list.Where(c => c.TypeOfReading.Equals((BO.TypeOfReading)filterValue)),
                BO.CallInListFields.LastVolunteerName => list.Where(c => c.LastVolunteerName != null && c.LastVolunteerName.Contains(filterValue.ToString(), StringComparison.OrdinalIgnoreCase)),
                BO.CallInListFields.Status => list.Where(c => c.Status.Equals(filterValue.ToString())),
                BO.CallInListFields.OpeningTime => list.Where(c => c.OpeningTime.Equals(Convert.ToDateTime(filterValue))),
                BO.CallInListFields.TotalAssignments => list.Where(c => c.TotalAssignments.Equals(Convert.ToInt32(filterValue))),
                _ => list
            };
        }

        list = sortBy switch
        {
            BO.CallInListFields.AssignmentId => list.OrderBy(c => c.AssignmentId),
            BO.CallInListFields.CallId => list.OrderBy(c => c.CallId),
            BO.CallInListFields.TypeOfReading => list.OrderBy(c => c.TypeOfReading),
            BO.CallInListFields.LastVolunteerName => list.OrderBy(c => c.LastVolunteerName),
            BO.CallInListFields.Status => list.OrderBy(c => c.Status),
            BO.CallInListFields.OpeningTime => list.OrderBy(c => c.OpeningTime),
            BO.CallInListFields.TotalAssignments => list.OrderBy(c => c.TotalAssignments),
            _ => list.OrderBy(c => c.CallId)
        };

        return list;
    }

    #region Stage 5
    public void AddObserver(Action listObserver) =>
        CallManager.Observers.AddListObserver(listObserver);
    public void AddObserver(int id, Action observer) =>
        CallManager.Observers.AddObserver(id, observer);
    public void RemoveObserver(Action listObserver) =>
        CallManager.Observers.RemoveListObserver(listObserver);
    public void RemoveObserver(int id, Action observer) =>
        CallManager.Observers.RemoveObserver(id, observer);
    #endregion Stage 5
}
