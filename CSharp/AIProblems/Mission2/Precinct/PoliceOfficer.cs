namespace Precinct;

public class PoliceOfficer
{
    // Protected means these are hidden from the outside world, 
    // but the specialized child classes CAN see them!
    protected string Name;
    protected string BadgeNumber;

    public PoliceOfficer(string name, string badgeNumber)
    {
        Name = name;
        BadgeNumber = badgeNumber;
    }

    public string GetName() { return Name; }
    public string GetBadge() { return BadgeNumber; }

    // The 'virtual' keyword means "Child classes are allowed to change how this works"
    public virtual string PerformDuty()
    {
        return $"{Name} is filling out standard precinct paperwork.";
    }
}
