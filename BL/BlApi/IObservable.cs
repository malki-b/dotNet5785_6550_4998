namespace BlApi;

/// &lt;summary&gt;
/// This interface provides actions to register (add) and unregister (remove) observers
/// for changes in a list of entities and in a speecific entity
/// &lt;/summary&gt;
public interface IObservable //stage 5
{
    /// &lt;summary&gt;
    /// Register observer for changes in a list of entities
    /// &lt;/summary&gt;
    /// &lt;param name="listObserver"&gt;the observer method to be registered&lt;/param&gt;
    void AddObserver(Action listObserver);
    /// &lt;summary&gt;
    /// Register observer for changes in a specific entity instance
    /// &lt;/summary&gt;
    /// &lt;param name="id"&gt;the identifier of the entity instance to be observed&lt;/param&gt;
    /// &lt;param name="observer"&gt;the observer method to be registered&lt;/param&gt;
    void AddObserver(int id, Action observer);
    /// &lt;summary&gt;
    /// Unregister observer for changes in a list of entities
    /// &lt;/summary&gt;
    /// &lt;param name="listObserver"&gt;the observer method to be unregistered&lt;/param&gt;
    void RemoveObserver(Action listObserver);
    /// &lt;summary&gt;
    /// Unregister observer for changes in a specific entity instance
    /// &lt;/summary&gt;
    /// &lt;param name="id"&gt;the identifier of the entity instance that was observed&lt;/param&gt;
    /// &lt;param name="observer"&gt;the observer method to be unregistered&lt;/param&gt;
    void RemoveObserver(int id, Action observer);
}