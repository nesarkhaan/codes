using System.Collections.Generic;

namespace PressRoom;

public class PressQuestion
{
    private string QuestionText;
    private List<string> Answers;

    public PressQuestion(string questionText)
    {
        QuestionText = questionText;
        Answers = new List<string>();
    }

    public void AddAnswer(string answer)
    {
        Answers.Add(answer);
    }

    public string GetQuestion() { return QuestionText; }
    public List<string> GetAnswers() { return Answers; }
}
