namespace SmartHome;

public class SmartDevice
{
    private string name;
    private bool isOn;
    private int batteryLevel; // 0 to 100

    public SmartDevice(string name, int batteryLevel)
    {
        this.name = name;
        this.batteryLevel = batteryLevel;
        
        
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
