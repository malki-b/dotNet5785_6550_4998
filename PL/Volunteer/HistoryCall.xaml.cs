using BlApi;
using BO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace PL.Volunteer
{
    public partial class HistoryCall : Window, INotifyPropertyChanged
    {
        private static readonly IBl s_bl = Factory.Get();
        private volatile DispatcherOperation? _observerOperation = null;

        public HistoryCall(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;
            DataContext = this;
            RefreshCallHistory(); // טעינה ראשונית
        }

        public int VolunteerId { get; }

        private IEnumerable<ClosedCallInList> _callHistoryList = Enumerable.Empty<ClosedCallInList>();
        public IEnumerable<ClosedCallInList> CallHistoryList
        {
            get => _callHistoryList;
            set
            {
                _callHistoryList = value;
                OnPropertyChanged();
            }
        }

        private TypeOfReading _typeOfCallFilterProp = TypeOfReading.None;
        public TypeOfReading TypeOfCallFilterProp
        {
            get => _typeOfCallFilterProp;
            set
            {
                _typeOfCallFilterProp = value;
                OnPropertyChanged();
                RefreshCallHistory();
            }
        }

        private ClosedCallField _callSortProp = ClosedCallField.None;
        public ClosedCallField CallSortProp
        {
            get => _callSortProp;
            set
            {
                _callSortProp = value;
                OnPropertyChanged();
                RefreshCallHistory();
            }
        }

        // === Observer-Style Loader ===
        private void RefreshCallHistory()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
            {
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    //var calls = s_bl.Call.RequestClosedCallsByVolunteer(
                    //    volunteerId: VolunteerId,
                    //    TypeOfReading: TypeOfCallFilterProp == TypeOfReading.None ? null : TypeOfCallFilterProp,
                    //    sortField: CallSortProp == ClosedCallField.None ? null : CallSortProp
                    //);

                    //CallHistoryList = calls.ToList();
                });
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) =>
            s_bl.Volunteer.AddObserver(RefreshCallHistory);

        private void Window_Closed(object sender, EventArgs e) =>
            s_bl.Volunteer.RemoveObserver(RefreshCallHistory);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
