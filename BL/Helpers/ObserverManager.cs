namespace Helpers
{
    /// <summary>
    /// This class is a helper class allowing to manage observers for different logical entities
    /// in the Business Logic (BL) layer.
    /// It offers infrastructure to support observers as follows:
    /// <list type="bullet">
    /// <item>An event delegate for list observers - wherever there may be a change in the
    /// presentation of the list of entities</item>
    /// <item>A hash table of delegates for individual entity observers - indexed by appropriate entity ID</item>
    /// </list>
    /// </summary>
    class ObserverManager //stage 5
    {
        /// <summary>
        /// Event delegate for list observers - it's called whenever there may be need to update the presentation
        /// of the list of entities.
        /// </summary>
        private event Action? _listObservers;

        /// <summary>
        /// Hash table (Dictionary) of individual entity delegates.<br/>
        /// The index (key) is the ID of an entity.<br/>
        /// If there are no observers for a specific entity instance - there will not be entry in the hash
        /// table for it, thus providing memory effective storage for these observers.
        /// </summary>
        private readonly Dictionary<int, Action?> _specificObservers = new();

        /// <summary>
        /// Add an observer on change in list of entities that may affect the list presentation.
        /// </summary>
        /// <param name="observer">Observer method (usually from Presentation Layer) to be added.</param>
        internal void AddListObserver(Action observer) => _listObservers += observer;

        /// <summary>
        /// Remove an observer on change in list of entities that may affect the list presentation.
        /// </summary>
        /// <param name="observer">Observer method (usually from Presentation Layer) to be removed.</param>
        internal void RemoveListObserver(Action observer) => _listObservers -= observer;

        /// <summary>
        /// Add an observer on change in an instance of entity that may affect the entity instance presentation.
        /// </summary>
        /// <param name="id">The ID value for the entity instance to be observed.</param>
        /// <param name="observer">Observer method (usually from Presentation Layer) to be added.</param>
        internal void AddObserver(int id, Action observer)
        {
            if (_specificObservers.ContainsKey(id)) // If there are already observers for the ID
                _specificObservers[id] += observer; // Add the given observer
            else // This is the first observer for the ID
                _specificObservers[id] = observer; // Create hash table entry for the ID with the given observer
        }

        /// <summary>
        /// Remove an observer on change in an instance of entity that may affect the entity instance presentation.
        /// </summary>
        /// <param name="id">The ID value for the observed entity instance.</param>
        /// <param name="observer">Observer method (usually from Presentation Layer) to be removed.</param>
        internal void RemoveObserver(int id, Action observer)
        {
            // First, check that there are any observers for the ID
            if (_specificObservers.ContainsKey(id
