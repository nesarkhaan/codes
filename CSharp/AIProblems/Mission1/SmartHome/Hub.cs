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
        
        for(int i = activeDevices; i < network.Length; i++)
        
        if (i <= network.Length)
        {
            network[i] = device;
            activeDevices++;
            return true;
        }
        
        
        // TODO: Add the device if there is room. Return true if successful, false if full.
        return false;
    }

    public void TurnOffAll()
    {
        // TODO: Loop through all active devices and call their TurnOff() method.
        for(int i = 0; i < activeDevices;i++)
        {
            network[i].TurnOff();
        
        
        }
    }

    public SmartDevice GetLowestBattery()
    {
        
        int calc = activeDevices;
        for(int i = 0; i < activeDevices;i++)
        {
            for(int j = 0; j < activeDevices; j--)
                  
            {
                  if (network[i] != network)
            
            
            }
        
        
        }
        // TODO: Find and return the device with the lowest battery percentage.
        // Return null if there are no devices on the network.
        return null;
    }
}
