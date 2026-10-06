using Xunit;
using SmartHome;

namespace SmartHome.Tests;

public class HubTests
{
    [Fact]
    public void AddDevice_ReturnsTrue_WhenSpaceAvailable()
    {
        Hub hub = new Hub(2);
        SmartDevice light = new SmartDevice("Living Room Light", 100);
        
        bool result = hub.AddDevice(light);
        
        Assert.True(result);
    }

    [Fact]
    public void TurnOffAll_TurnsOffEveryActiveDevice()
    {
        Hub hub = new Hub(3);
        SmartDevice light1 = new SmartDevice("Light 1", 100);
        SmartDevice light2 = new SmartDevice("Light 2", 100);
        
        light1.TurnOn();
        light2.TurnOn();
        
        hub.AddDevice(light1);
        hub.AddDevice(light2);
        
        hub.TurnOffAll();
        
        Assert.False(light1.GetIsOn());
        Assert.False(light2.GetIsOn());
    }

    [Fact]
    public void GetLowestBattery_ReturnsDeviceWithLowestCharge()
    {
        Hub hub = new Hub(3);
        SmartDevice thermostat = new SmartDevice("Thermostat", 80);
        SmartDevice lockDevice = new SmartDevice("Front Door", 15);
        SmartDevice camera = new SmartDevice("Camera", 45);
        
        hub.AddDevice(thermostat);
        hub.AddDevice(lockDevice);
        hub.AddDevice(camera);
        
        SmartDevice lowest = hub.GetLowestBattery();
        
        Assert.Equal("Front Door", lowest.GetName());
    }
}
