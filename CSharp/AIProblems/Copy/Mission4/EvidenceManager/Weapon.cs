namespace EvidenceManager;

public class Weapon
{
    private string SerialNumber;

    public Weapon(string serialNumber)
    {
        SerialNumber = serialNumber;
    }

    public string GetSerialNumber() { return SerialNumber; }
}
