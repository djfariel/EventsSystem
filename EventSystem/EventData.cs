namespace EventSystem;


public struct EventData {
    public string EventKey;
    public string EventNamespace;
    public int EventValue;

    public EventData(string eventKey, string eventNamespace, int eventValue)
    {
        EventKey = eventKey;
        EventNamespace = eventNamespace;
        EventValue = eventValue;
    }
}