using System.IO;
using System.Collections.Generic;

namespace PressRoom;

public class BriefingManager
{
    public List<PressQuestion> LoadStructuredQA(string filePath)
    {
        List<PressQuestion> briefingDeck = new List<PressQuestion>();
        string[] lines = File.ReadAllLines(filePath);
        
        PressQuestion currentQuestion = null;

        foreach (string line in lines)
        {
            // Skip empty lines so your program doesn't crash
            if (string.IsNullOrWhiteSpace(line)) continue;

            // TODO: If the line starts with "Q: "
            // 1. Create a new PressQuestion object (use .Substring(3) to remove the "Q: " part of the string!)
            // 2. Add it to the briefingDeck list.
            // 3. Make currentQuestion point to this new object.

            // TODO: Else If the line starts with "A: "
            // 1. Make sure currentQuestion is not null (safety check!)
            // 2. Add the answer to currentQuestion (again, use .Substring(3) to remove "A: ")
        }

        return briefingDeck;
    }
}
