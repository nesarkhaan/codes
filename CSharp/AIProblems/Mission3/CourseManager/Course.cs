using System.Collections.Generic;

namespace CourseManager;

public class Course
{
    private string CourseName;
    // Here is your dynamic list! Notice it is initialized right in the constructor.
    private List<Student> Roster;

    public Course(string courseName)
    {
        CourseName = courseName;
        Roster = new List<Student>(); 
    }

    public void Enroll(Student newStudent)
    {
        // TODO: Add the new student to the Roster
    }

    public int GetClassSize()
    {
        // TODO: Return the current number of students in the list
        return 0;
    }

    public bool DropOut(string studentName)
    {
        // TODO: Find the student by name, remove them from the Roster, and return true.
        // Return false if they are not found.
        return false;
    }

    public Student GetOldestStudent()
    {
        // TODO: Find and return the student with the highest age.
        // Return null if Roster.Count == 0.
        return null;
    }
}
