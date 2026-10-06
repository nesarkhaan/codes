# Mission 2: The Precinct Roster (Inheritance)

**The Goal:** You are building a system to manage different types of police officers using Object-Oriented Inheritance. You have a base `PoliceOfficer` class, and you need to create specialized `PatrolOfficer` and `Detective` classes that inherit from it.

**Your Tasks:**
1. Create `PatrolOfficer.cs` that inherits from `PoliceOfficer`.
   - Add a private `PatrolZone` string field.
   - Write a constructor that takes name, badge number, and patrol zone. You must pass the name and badge up to the base class!
   - Override the `PerformDuty()` method to return: "[Name] is actively patrolling zone [PatrolZone]."
   
2. Create `Detective.cs` that inherits from `PoliceOfficer`.
   - Add a private `IsUndercover` boolean field.
   - Write a constructor that takes name, badge number, and undercover status.
   - Override the `PerformDuty()` method. If undercover, return: "[Name] is gathering evidence in secret." If not, return: "[Name] is interviewing witnesses."
