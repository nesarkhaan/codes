namespace Precinct;

// The colon ':' means PatrolOfficer INHERITS from PoliceOfficer
public class PatrolOfficer : PoliceOfficer
{
    private string PatrolZone;

    // TODO: Write the constructor!
    // Hint: You must pass the name and badge up to the base class using the 'base' keyword.
    // public PatrolOfficer(string name, string badgeNumber, string patrolZone) : base(...) { ... }

    // TODO: Override the PerformDuty method.
    // It should return: "[Name] is actively patrolling zone [PatrolZone]."
    // Hint: Use the 'override' keyword.
}
