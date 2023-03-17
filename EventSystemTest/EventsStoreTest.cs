using EventSystem;

namespace EventSystemTest;


[TestClass]
public class EventsStoreTest
{

    EventsStore sut = new();

    [TestMethod]
    public void Test_SaveEventAndGetEvent()
    {
        string eventName = "testData";
        sut.SaveEvent(eventName);
        Assert.AreEqual(sut.GetEvent(eventName), 1);
    }

    [TestMethod]
    public void Test_SaveEvent_Twice()
    {
        string eventName = "testData";
        sut.SaveEvent(eventName);
        sut.SaveEvent(eventName);
        Assert.AreEqual(sut.GetEvent(eventName), 2);
    }

    [TestMethod]
    public void Test_SaveEvent_Namespaced()
    {
        string eventName = "testData";
        string eventNamespace = "testNamespace";
        sut.SaveEvent(eventName, eventNamespace);
        Assert.AreEqual(sut.GetEvent(eventName, eventNamespace), 1);
        Assert.AreNotEqual(sut.GetEvent(eventName), 1);
    }

    [TestMethod]
    public void Test_SaveEvent_NamespacedButSameNames()
    {
        string eventName = "testData";
        string eventNamespace1 = "testNamespace1";
        string eventNamespace2 = "testNamespace2";
        sut.SaveEvent(eventName, eventNamespace1);
        sut.SaveEvent(eventName, eventNamespace2);
        Assert.AreEqual(sut.GetEvent(eventName, eventNamespace1), 1);
        Assert.AreEqual(sut.GetEvent(eventName, eventNamespace2), 1);
        Assert.AreNotEqual(sut.GetEvent(eventName), 1);
    }

    [TestMethod]
    public void Test_AddToEvent_New()
    {
        string eventName = "testData";
        sut.AddToEvent(eventName, 5);
        Assert.AreEqual(sut.GetEvent(eventName), 5);
    }

    [TestMethod]
    public void Test_AddToEvent_Existing()
    {
        string eventName = "testData";
        sut.SaveEvent(eventName);
        sut.AddToEvent(eventName, 5);
        Assert.AreEqual(sut.GetEvent(eventName), 6);
    }

    [TestMethod]
    public void Test_AddToEvent_Namespaced()
    {
        string eventName = "testData";
        string eventNamespace = "testNamespace";
        sut.AddToEvent(eventName, 5, eventNamespace);
        Assert.AreNotEqual(sut.GetEvent(eventName), 5);
        Assert.AreEqual(sut.GetEvent(eventName, eventNamespace), 5);
    }

    [TestMethod]
    public void Test_RemoveFromEvent_New()
    {
        string eventName = "testData";
        sut.RemoveFromEvent(eventName, 5);
        Assert.AreEqual(sut.GetEvent(eventName), -5);
    }

    [TestMethod]
    public void Test_RemoveFromEvent_Existing()
    {
        string eventName = "testData";
        sut.SaveEvent(eventName);
        sut.RemoveFromEvent(eventName, 5);
        Assert.AreEqual(sut.GetEvent(eventName), -4);
    }

    [TestMethod]
    public void Test_RemoveFromEvent_Namespaced()
    {
        string eventName = "testData";
        string eventNamespace = "testNamespace";
        sut.RemoveFromEvent(eventName, 5, eventNamespace);
        Assert.AreNotEqual(sut.GetEvent(eventName), -5);
        Assert.AreEqual(sut.GetEvent(eventName, eventNamespace), -5);
    }

    [TestMethod]
    public void Test_SetEventValue_New()
    {
        string eventName = "testData";
        sut.SetEventValue(eventName, 69);
        Assert.AreEqual(sut.GetEvent(eventName), 69);
    }

    [TestMethod]
    public void Test_SetEventValue_Existing()
    {
        string eventName = "testData";
        sut.SaveEvent(eventName);
        sut.SetEventValue(eventName, 69);
        Assert.AreEqual(sut.GetEvent(eventName), 69);
    }

    [TestMethod]
    public void Test_EventHasValue_True()
    {
        string eventName = "testData";
        sut.SetEventValue(eventName, 69);
        Assert.AreEqual(sut.EventHasValue(eventName), true);
    }

    [TestMethod]
    public void Test_EventHasValue_False()
    {
        string eventName = "testData";
        Assert.AreEqual(sut.EventHasValue(eventName), false);
    }

    [TestMethod]
    public void Test_ClearNamespace()
    {
        sut.SaveEvent("event1");
        sut.SaveEvent("event2", "firstNamespace");
        sut.SaveEvent("event3", "secondNamespace");
        Assert.AreEqual(sut.EventHasValue("event1"), true);
        Assert.AreEqual(sut.EventHasValue("event2", "firstNamespace"), true);
        Assert.AreEqual(sut.EventHasValue("event3", "secondNamespace"), true);

        sut.ClearNamespace("secondNamespace");

        Assert.AreEqual(sut.EventHasValue("event1"), true);
        Assert.AreEqual(sut.EventHasValue("event2", "firstNamespace"), true);
        Assert.AreEqual(sut.EventHasValue("event3", "secondNamespace"), false);
    }

    [TestMethod]
    public void Test_ClearAllEvents()
    {
        sut.SaveEvent("event1");
        sut.SaveEvent("event2", "firstNamespace");
        sut.SaveEvent("event3", "secondNamespace");
        Assert.AreEqual(sut.EventHasValue("event1"), true);
        Assert.AreEqual(sut.EventHasValue("event2", "firstNamespace"), true);
        Assert.AreEqual(sut.EventHasValue("event3", "secondNamespace"), true);

        sut.ClearAllEvents();

        Assert.AreEqual(sut.EventHasValue("event1"), false);
        Assert.AreEqual(sut.EventHasValue("event2", "firstNamespace"), false);
        Assert.AreEqual(sut.EventHasValue("event3", "secondNamespace"), false);
    }

    [TestMethod]
    public void Test_GetSerializableData()
    {
        Dictionary<string, Dictionary<string, int>> data = new Dictionary<string, Dictionary<string, int>>
        {
            { "", new Dictionary<string, int> { { "event1", 2 } } },
            { "firstNamespace", new Dictionary<string, int> { { "event2", 1 } } },
            { "secondNamespace", new Dictionary<string, int> { { "event3", 1 } } },
        };

        sut.SaveEvent("event1");
        sut.SaveEvent("event1");
        sut.SaveEvent("event2", "firstNamespace");
        sut.SaveEvent("event3", "secondNamespace");
        Assert.AreEqual(DictionaryToString(sut.GetSerializableData()), DictionaryToString(data));
    }

    [TestMethod]
    public void Test_SetSerializableData()
    {
        Dictionary<string, Dictionary<string, int>> data = new Dictionary<string, Dictionary<string, int>>
        {
            { "", new Dictionary<string, int> { { "event1", 2 } } },
            { "firstNamespace", new Dictionary<string, int> { { "event2", 1 } } },
            { "secondNamespace", new Dictionary<string, int> { { "event3", 1 } } },
        };

        sut.SetSerializableData(data);

        Assert.AreEqual(DictionaryToString(sut.GetSerializableData()), DictionaryToString(data));
    }

    [TestMethod]
    public void Test_EventListener()
    {
        List<EventData> receivedEvents = new List<EventData>();
        sut.OnEventDataChanged += delegate(object? sender, EventData eventData)
        {
            receivedEvents.Add(eventData);
        };

        string eventName = "testData";
        sut.SaveEvent(eventName);

        Assert.AreEqual(receivedEvents.Count, 1);
        Assert.AreEqual(receivedEvents[0].EventKey, eventName);
        Assert.AreEqual(receivedEvents[0].EventNamespace, "");
        Assert.AreEqual(receivedEvents[0].EventValue, 1);
    }

    [TestMethod]
    public void Test_EventListener_MultipleEvents()
    {
        List<EventData> receivedEvents = new List<EventData>();
        sut.OnEventDataChanged += delegate(object? sender, EventData eventData)
        {
            receivedEvents.Add(eventData);
        };

        sut.SaveEvent("event1");
        sut.SaveEvent("event2", "firstNamespace");
        sut.SaveEvent("event3", "secondNamespace");

        Assert.AreEqual(receivedEvents.Count, 3);
        Assert.AreEqual(receivedEvents[0].EventKey, "event1");
        Assert.AreEqual(receivedEvents[0].EventNamespace, "");
        Assert.AreEqual(receivedEvents[0].EventValue, 1);
        Assert.AreEqual(receivedEvents[1].EventKey, "event2");
        Assert.AreEqual(receivedEvents[1].EventNamespace, "firstNamespace");
        Assert.AreEqual(receivedEvents[1].EventValue, 1);
        Assert.AreEqual(receivedEvents[2].EventKey, "event3");
        Assert.AreEqual(receivedEvents[2].EventNamespace, "secondNamespace");
        Assert.AreEqual(receivedEvents[2].EventValue, 1);
    }

    [TestMethod]
    public void Test_EventListener_ClearNamespace()
    {
        List<EventData> receivedEvents = new List<EventData>();
        sut.OnEventDataChanged += delegate(object? sender, EventData eventData)
        {
            receivedEvents.Add(eventData);
        };

        sut.SaveEvent("event1");
        sut.SaveEvent("event2", "firstNamespace");
        sut.SaveEvent("event3", "secondNamespace");
        sut.ClearNamespace("secondNamespace");

        Assert.AreEqual(receivedEvents.Count, 4);
        Assert.AreEqual(receivedEvents[0].EventKey, "event1");
        Assert.AreEqual(receivedEvents[0].EventNamespace, "");
        Assert.AreEqual(receivedEvents[0].EventValue, 1);
        Assert.AreEqual(receivedEvents[1].EventKey, "event2");
        Assert.AreEqual(receivedEvents[1].EventNamespace, "firstNamespace");
        Assert.AreEqual(receivedEvents[1].EventValue, 1);
        Assert.AreEqual(receivedEvents[2].EventKey, "event3");
        Assert.AreEqual(receivedEvents[2].EventNamespace, "secondNamespace");
        Assert.AreEqual(receivedEvents[2].EventValue, 1);
        Assert.AreEqual(receivedEvents[3].EventKey, "event3");
        Assert.AreEqual(receivedEvents[3].EventNamespace, "secondNamespace");
        Assert.AreEqual(receivedEvents[3].EventValue, 0);
    }

    [TestMethod]
    public void Test_EventListener_ClearEverything()
    {
        List<EventData> receivedEvents = new List<EventData>();
        sut.OnEventDataChanged += delegate(object? sender, EventData eventData)
        {
            receivedEvents.Add(eventData);
        };

        sut.SaveEvent("event1");
        sut.SaveEvent("event2", "firstNamespace");
        sut.SaveEvent("event3", "secondNamespace");
        sut.ClearAllEvents();

        Assert.AreEqual(receivedEvents.Count, 6);
        Assert.AreEqual(receivedEvents[0].EventKey, "event1");
        Assert.AreEqual(receivedEvents[0].EventNamespace, "");
        Assert.AreEqual(receivedEvents[0].EventValue, 1);
        Assert.AreEqual(receivedEvents[1].EventKey, "event2");
        Assert.AreEqual(receivedEvents[1].EventNamespace, "firstNamespace");
        Assert.AreEqual(receivedEvents[1].EventValue, 1);
        Assert.AreEqual(receivedEvents[2].EventKey, "event3");
        Assert.AreEqual(receivedEvents[2].EventNamespace, "secondNamespace");
        Assert.AreEqual(receivedEvents[2].EventValue, 1);
        Assert.AreEqual(receivedEvents[3].EventKey, "event1");
        Assert.AreEqual(receivedEvents[3].EventNamespace, "");
        Assert.AreEqual(receivedEvents[3].EventValue, 0);
        Assert.AreEqual(receivedEvents[4].EventKey, "event2");
        Assert.AreEqual(receivedEvents[4].EventNamespace, "firstNamespace");
        Assert.AreEqual(receivedEvents[4].EventValue, 0);
        Assert.AreEqual(receivedEvents[5].EventKey, "event3");
        Assert.AreEqual(receivedEvents[5].EventNamespace, "secondNamespace");
        Assert.AreEqual(receivedEvents[5].EventValue, 0);
    }

    ////////////////////////////////////// helper methods /////////////////////////////////////////////

    string DictionaryToString(Dictionary<string, Dictionary<string, int>> dictionary)
    {
        string dictionaryString = "{";
        foreach(KeyValuePair<string, Dictionary<string, int>> keyValues in dictionary) {
            dictionaryString += keyValues.Key + " : " + DictionaryToString(keyValues.Value) + ", ";
        }
        return dictionaryString.TrimEnd(',', ' ') + "}";
    }

    string DictionaryToString(Dictionary<string, int> dictionary)
    {
        string dictionaryString = "{";
        foreach(KeyValuePair<string, int> keyValues in dictionary) {
            dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";
        }
        return dictionaryString.TrimEnd(',', ' ') + "}";
    }
}