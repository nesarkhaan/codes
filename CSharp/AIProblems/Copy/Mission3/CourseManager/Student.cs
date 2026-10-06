namespace CourseManager;

public class Student
{
    private string Name;
    private int Age;

    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string GetName() { return Name; }
    public int GetAge() { return Age; }
}
