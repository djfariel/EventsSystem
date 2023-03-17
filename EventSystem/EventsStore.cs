namespace EventSystem;



/// <summary>
/// Class <c>EventsStore</c> keeps a record of all events that happen in a game and is able to return a serialized dictionary if asked.
/// </summary>
public class EventsStore
{
    private Dictionary<string, Dictionary<string, int>> _savedEvents = new();

    /// <summary>
    /// Event Handler for event data changes. Will send an EventData struct out every time any value changes.
    /// </summary>
    public event EventHandler<EventData>? OnEventDataChanged;

    /// <summary>
    /// Adds an event to the event store. This will set the value to 1 if the event does not exist already, otherwise will increment the value of the event.
    /// </summary>
    /// <param name="key">The key, or name, of the event. This is how you will retrieve the value later.</param>
    /// <param name="eventNamespace">Optional, a namespace with which you can segregate your events.</param>
    /// <returns>The current value of the event.</returns>
    public int SaveEvent(string key, string eventNamespace = "")
    {
        if (!_savedEvents.ContainsKey(eventNamespace))
        {
            _savedEvents.Add(eventNamespace, new Dictionary<string, int>());
        }

        if (!_savedEvents[eventNamespace].ContainsKey(key))
        {
            _savedEvents[eventNamespace].Add(key, 1);
        }
        else
        {
            _savedEvents[eventNamespace][key]++;
        }

        EventDataChangedHandler(new EventData(key, eventNamespace, _savedEvents[eventNamespace][key]));
        return _savedEvents[eventNamespace][key];
    }

    /// <summary>
    /// Adds a value to an existing event if the event exists. Otherwise, creates a new event with the given value.
    /// </summary>
    /// <param name="key">The key, or name, of the event. This is how you will retrieve the value later.</param>
    /// <param name="amount">The value with which to add to the event.</param>
    /// <param name="eventNamespace">Optional, a namespace with which you can segregate your events.</param>
    /// <returns>The current value of the event.</returns>
    public int AddToEvent(string key, int amount, string eventNamespace = "")
    {
        if (!_savedEvents.ContainsKey(eventNamespace))
        {
            _savedEvents.Add(eventNamespace, new Dictionary<string, int>());
        }

        if (!_savedEvents[eventNamespace].ContainsKey(key))
        {
            _savedEvents[eventNamespace].Add(key, amount);
        }
        else
        {
            _savedEvents[eventNamespace][key] += amount;
        }

        EventDataChangedHandler(new EventData(key, eventNamespace, _savedEvents[eventNamespace][key]));
        return _savedEvents[eventNamespace][key];
    }

    /// <summary>
    /// Removes a value from an existing event if the event exists. Otherwise, creates a new event with the given value.
    /// </summary>
    /// <param name="key">The key, or name, of the event. This is how you will retrieve the value later.</param>
    /// <param name="amount">The value with which to remove from the event.</param>
    /// <param name="eventNamespace">Optional, a namespace with which you can segregate your events.</param>
    /// <returns>The current value of the event.</returns>
    public int RemoveFromEvent(string key, int amount, string eventNamespace = "")
    {
        if (!_savedEvents.ContainsKey(eventNamespace))
        {
            _savedEvents.Add(eventNamespace, new Dictionary<string, int>());
        }

        if (!_savedEvents[eventNamespace].ContainsKey(key))
        {
            _savedEvents[eventNamespace].Add(key, -amount);
        }
        else
        {
            _savedEvents[eventNamespace][key] -= amount;
        }

        EventDataChangedHandler(new EventData(key, eventNamespace, _savedEvents[eventNamespace][key]));
        return _savedEvents[eventNamespace][key];
    }

    /// <summary>
    /// Sets the value of an existing event if the event exists. Otherwise, creates a new event with the given value.
    /// </summary>
    /// <param name="key">The key, or name, of the event. This is how you will retrieve the value later.</param>
    /// <param name="amount">The value with which to set the event to.</param>
    /// <param name="eventNamespace">Optional, a namespace with which you can segregate your events.</param>
    /// <returns>The current value of the event.</returns>
    public int SetEventValue(string key, int amount, string eventNamespace = "")
    {
        if (!_savedEvents.ContainsKey(eventNamespace))
        {
            _savedEvents.Add(eventNamespace, new Dictionary<string, int>());
        }

        if (!_savedEvents[eventNamespace].ContainsKey(key))
        {
            _savedEvents[eventNamespace].Add(key, amount);
        }
        else
        {
            _savedEvents[eventNamespace][key] = amount;
        }

        EventDataChangedHandler(new EventData(key, eventNamespace, _savedEvents[eventNamespace][key]));
        return _savedEvents[eventNamespace][key];
    }

    /// <summary>
    /// Gets the current value of an event. If an event does not exist, its value is 0.
    /// </summary>
    /// <param name="key">The key, or name, of the event. This is how you will retrieve the value later.</param>
    /// <param name="eventNamespace">Optional, a namespace with which you can segregate your events.</param>
    /// <returns>The current value of the event if it exists, otherwise 0.</returns>
    public int GetEvent(string key, string eventNamespace = "")
    {
        if (!_savedEvents.ContainsKey(eventNamespace)) return 0;
        if (!_savedEvents[eventNamespace].ContainsKey(key)) return 0;
        return _savedEvents[eventNamespace][key];
    }

    /// <summary>
    /// Gets whether an event has a non-zero value.
    /// </summary>
    /// <param name="key">The key, or name, of the event. This is how you will retrieve the value later.</param>
    /// <param name="eventNamespace">Optional, a namespace with which you can segregate your events.</param>
    /// <returns>True if the event has a non-zero value. False if the event has a value of zero or does not exist.</returns>
    public bool EventHasValue(string key, string eventNamespace = "")
    {
        if (!_savedEvents.ContainsKey(eventNamespace)) return false;
        if (!_savedEvents[eventNamespace].ContainsKey(key)) return false;
        return _savedEvents[eventNamespace][key] != 0;
    }

    /// <summary>
    /// Clears all events in a given namespace. This removes data!
    /// </summary>
    /// <param name="eventNamespace">The namespace you would like to clear.</param>
    /// <returns>True if the namespace was cleared, false if the namespace was not found.</returns>
    public bool ClearNamespace(string eventNamespace)
    {
        foreach(string key in _savedEvents[eventNamespace].Keys)
        {
            EventDataChangedHandler(new EventData(key, eventNamespace, 0));
        }
        return _savedEvents.Remove(eventNamespace);
    }

    /// <summary>
    /// Clears all events. All of them. This removes data!
    /// </summary>
    public void ClearAllEvents()
    {
        foreach(string eventNamespace in _savedEvents.Keys)
        {
            foreach(string key in _savedEvents[eventNamespace].Keys)
            {
                EventDataChangedHandler(new EventData(key, eventNamespace, 0));
            }
        }
        _savedEvents.Clear();
    }

    /// <summary>
    /// Gets a dictionary which you can then use to persist your data.
    /// </summary>
    /// <returns>Your event data.</returns>
    public Dictionary<string, Dictionary<string, int>> GetSerializableData()
    {
        return _savedEvents;
    }

    /// <summary>
    /// Set event data, usually from some form of persistance layer. This will overwrite any currently stored data.
    /// </summary>
    public void SetSerializableData(Dictionary<string, Dictionary<string, int>> newData)
    {
        _savedEvents = newData;
        foreach(string eventNamespace in _savedEvents.Keys)
        {
            foreach(KeyValuePair<string, int> keyValues in _savedEvents[eventNamespace])
            {
                EventDataChangedHandler(new EventData(keyValues.Key, eventNamespace, keyValues.Value));
            }
        }
    }

    /// <summary>
    /// Passes event data to listeners of the changed state.
    /// </summary>
    private void EventDataChangedHandler(EventData eventData)
    {
        OnEventDataChanged?.Invoke(this, eventData);
    }
}