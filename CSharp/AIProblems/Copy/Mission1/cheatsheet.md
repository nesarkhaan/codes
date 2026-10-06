# Cheat Sheet & Hints

* **AddDevice:** Remember your `Group` project! Check if the `activeDevices` counter is equal to the `network.Length`. If not, add it to the `network[activeDevices]` slot and increment the counter.
* **TurnOffAll:** Write a `for` loop that goes up to `activeDevices`. For each device, call `network[i].TurnOff()`.
* **GetLowestBattery:** 1. Create a variable called `lowestDevice` and set it to `network[0]` (assume the first one is the lowest to start).
  2. Loop through the rest of the array.
  3. If `network[i].GetBatteryLevel()` is LESS than `lowestDevice.GetBatteryLevel()`, update `lowestDevice = network[i]`.
  4. Return `lowestDevice` at the end!
