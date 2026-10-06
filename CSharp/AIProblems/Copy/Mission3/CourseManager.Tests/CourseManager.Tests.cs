using Xunit;
using CourseManager;

namespace CourseManager.Tests;

public class CourseTests
{
    [Fact]
    public void Enroll_AddsStudentToList()
    {
        Course course = new Course("Computer Basics");
        course.Enroll(new Student("Arthur", 72));
        course.Enroll(new Student("Betty", 68));

        Assert.Equal(2, course.GetClassSize());
    }

    [Fact]
    public void DropOut_RemovesStudent_ReturnsTrue()
    {
        Course course = new Course("iPad Fundamentals");
        Student s1 = new Student("Charles", 80);
        Student s2 = new Student("Diana", 75);
        
        course.Enroll(s1);
        course.Enroll(s2);

        bool result = course.DropOut("Charles");

        Assert.True(result);
        Assert.Equal(1, course.GetClassSize());
    }

    [Fact]
    public void GetOldestStudent_ReturnsCorrectStudent()
    {
        Course course = new Course("Internet Safety");
        course.Enroll(new Student("Evelyn", 70));
        course.Enroll(new Student("Frank", 85));
        course.Enroll(new Student("Grace", 81));

        Student oldest = course.GetOldestStudent();

        Assert.Equal("Frank", oldest.GetName());
    }
}
