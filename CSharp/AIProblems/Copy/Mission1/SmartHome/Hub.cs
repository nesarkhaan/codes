namespace SmartHome;

public class Hub
{
    private SmartDevice[] network;
    private int activeDevices;

    public Hub(int capacity)
    {
        network = new SmartDevice[capacity];
        activeDevices = 0;
    }

    public bool AddDevice(SmartDevice device)
    {
        // TODO: Add the device if there is room. Return true if successful, false if full.
        return false;
    }

    public void TurnOffAll()
    {
        // TODO: Loop through all active devices and call their TurnOff() method.
    }

    public SmartDevice GetLowestBattery()
    {
        // TODO: Find and return the device with the lowest battery percentage.
        // Return null if there are no devices on the network.
        return null;
    }
}
