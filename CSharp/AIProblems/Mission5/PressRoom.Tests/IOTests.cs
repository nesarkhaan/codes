using Xunit;
using PressRoom;
using System.IO;
using System.Collections.Generic;

namespace PressRoom.Tests;

public class IOTests
{
    [Fact]
    public void BriefingManager_ParsesStructuredDataCorrectly()
    {
        // 1. Set up a dummy file
        string testPath = "test_qa.txt";
        File.WriteAllText(testPath, "Q: Is water wet?\nA: Fake news.\nA: The wettest, from the standpoint of water.\n\nQ: Why?\nA: Because I said so.");

        BriefingManager manager = new BriefingManager();
        List<PressQuestion> deck = manager.LoadStructuredQA(testPath);

        // 2. Verify it created exactly 2 Question objects
        Assert.Equal(2, deck.Count);
        
        // 3. Verify the first question stripped the "Q: " and stored the text
        Assert.Equal("Is water wet?", deck[0].GetQuestion());
        
        // 4. Verify the first question loaded exactly 2 answers
        Assert.Equal(2, deck[0].GetAnswers().Count);
        Assert.Equal("Fake news.", deck[0].GetAnswers()[0]);

        File.Delete(testPath);
    }
}
