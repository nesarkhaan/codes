# Cheat Sheet: Inheritance & Overrides

**1. The `base` Keyword (Constructors):**
When a child class needs to set properties that actually belong to the parent class, it doesn't set them itself. It passes those variables "up the chain" to the parent's constructor using `: base(var1, var2)`.

*Example of how to write the constructor:*
public PatrolOfficer(string name, string badgeNumber, string patrolZone) 
    : base(name, badgeNumber) // This hands the name and badge up to the PoliceOfficer class!
{
    PatrolZone = patrolZone; // Then you handle the variable that only belongs to this child class
}


**2. The `override` Keyword (Methods):**
Because the parent class marked `PerformDuty()` as `virtual` (which means "I am granting permission for my children to change this"), the child class must use the `override` keyword to actually implement the change.

*Example of how to override a method:*
public override string PerformDuty()
{
    // Notice you can use the 'Name' variable here even though you didn't declare it in this file! 
    // That is because it was marked 'protected' in the base class, making it visible to children.
    return $"{Name} is actively patrolling zone {PatrolZone}."; 
}
