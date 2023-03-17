## EventSystem
Game event saving system for C# games. Probably most useful in Unity games, but can be used anywhere.
The general philosophy of this tool is that each event is unique. However, sometimes events can be really similar. For example, opening a chest can be a really repetitive event, and just naming your event key `openedChest` wouldn't be useful for very long at all. There are two features to help with this.
First, all events can be optionally namespaced. Every function has an optional namespace parameter that allows you to bucket events however you decide. For example, if you have one chest in each location, there are now two ways to handle this. You can have individual keys, like `openedChestForest` and `openedChestCastle`, or you can lean on the namespacing feature and continue to use `openedChest` in the `forest` and `castle` namespaces. The namespacing feature should be transparent and entirely optional, but it can be a powerful tool if needed.
Second, not all events are true or false. For example, you may record that the player found a specific `chicken feather` item, but may also want to know how many in total have been collected. To help with this, every event is saved as an integer. When you save an event, its value internally is increased from 0 to 1. Every additional time you save it, the value will increase by 1. There are also functions to increase by more than one, decrease, and set a specific value; just in case.
## Quick Use
 - Create an instance of `EventsStore`. This reference can follow a singleton pattern, just make sure you clean up your data responsibly.
 - Call `EventsStore.SaveEvent` and pass in the name of the event you would like to save as the key.
 - Call `EventsStore.GetEvent` and pass in the name of the event you set previously to recover its value.
 - Call `EventsStore.HasValue` and pass in the name of an event to test whether
   or not it has been set.

All functions have full documentation.

## Advanced Use
| Function | Description |
|--|--|
| SaveEvent | Saves an event under the specified key |
| AddToEvent| Adds an amount to an event |
| RemoveFromEvent| Removes an amount from an event |
| SetEventValue| Sets an event to a specific value |
| GetEvent| Gets the value of an event, or 0 |
| EventHasValue| Returns true if an event has a non-zero value |
| ClearNamespace| Removes all events in a namespace |
| ClearAllEvents| Removes all events |
| GetSerializableData| Gets the stores event data, which you can use to serialize |
| SetSerializableData| Replaces the current event data with new event data |

Objects that need to know when data changes can listen to `OnEventDataChanged`. This will return an `EventData` instance with the key, namespace, and value of changed data as each event occurs. Make sure your handler filters this data to only listen to events you care about!

```
Quick examples

EventsStore eventsStore = new EventsStore();

function YourFunction() {

    // save an event
    eventsStore.SaveEvent("somethingHappened!");

    // save an event in a namespace
    eventsStore.SaveEvent("somethingHappened!", "In Antarctica!");

    // get the value of an event from a namespace
    eventsStore.GetEvent("turkeyFeathersCollected", "forest");

    // check whether an event has occurred or not
    eventsStore.EventHasValue("tutorialFinished");

    // clear all events from the "gambling" namespaced
    eventsStore.ClearNamespace("gambling");
}
```