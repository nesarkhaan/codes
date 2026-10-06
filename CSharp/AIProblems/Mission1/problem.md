# Mission 1: The Smart Home Hub

**The Goal:** You are writing the software for a central Smart Home Hub. It manages an array of smart devices (lights, thermostats, locks). 

**Your Tasks:**
1. Complete the `SmartDevice` class constructor.
2. Complete the `Hub` class methods:
   - `AddDevice(SmartDevice device)`: Add a device to the network if there is room.
   - `TurnOffAll()`: Loop through all devices and set their `IsOn` property to false.
   - `GetLowestBattery()`: Loop through the devices and return the specific device object that has the lowest battery percentage.
