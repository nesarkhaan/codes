using System;
using System.Collections.Generic;

namespace PressRoom;

public class President
{
    public string AnswerSpecificQuestion(PressQuestion qaBlock)
    {
        List<string> possibleAnswers = qaBlock.GetAnswers();
        
        // TODO: Use the Random class to generate a number between 0 and possibleAnswers.Count
        // TODO: Return the string at that random index!
        return "";
    }
}
