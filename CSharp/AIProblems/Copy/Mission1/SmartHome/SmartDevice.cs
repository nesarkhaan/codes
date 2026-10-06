namespace SmartHome;

public class SmartDevice
{
    private string name;
    private bool isOn;
    private int batteryLevel; // 0 to 100

    public SmartDevice(string name, int batteryLevel)
    {
        // TODO: Assign the parameters to the fields. 
        // All devices should start with isOn = false.
        
    }

    public string GetName() { return name; }
    public int GetBatteryLevel() { return batteryLevel; }
    public bool GetIsOn() { return isOn; }

    public void TurnOff()
    {
        isOn = false;
    }
    
    public void TurnOn()
    {
        isOn = true;
    }
}
