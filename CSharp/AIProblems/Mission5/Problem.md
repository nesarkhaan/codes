# Mission 5: The Contextual Press Briefing (Data Parsing & System.IO)

**The Goal:** You have a massive text file (`qa_data.txt`) containing over 100 questions and 400+ highly detailed answers (heavily featuring tariffs and massive disappointments). The data is structured: Questions start with "Q: " and Answers start with "A: ". You must parse this text file into a list of C# Objects, conduct a mock press briefing, and save the official transcript to a new file.

**Your Tasks:**
1. **The Data Object:** Review the `PressQuestion` class. It holds a single `string` for the question and a `List<string>` for the possible answers.
2. **The Parser (`BriefingManager.LoadStructuredQA`):** - Read all lines from `qa_data.txt`.
   - Loop through the lines. 
   - If a line starts with "Q: ", create a new `PressQuestion` object, strip the "Q: " prefix, and add the object to your main `briefingDeck` list.
   - If a line starts with "A: ", strip the "A: " prefix and add the answer to the *current* `PressQuestion` object's list of answers.
3. **The Logic (`President.AnswerSpecificQuestion`):** Write the method that takes in a `PressQuestion` object, looks at its list of answers, and returns one of them at random.
4. **The Output (`BriefingManager.PublishTranscript`):** Take a generated list of strings representing the Q&A session and write them to a brand new text file called `transcript.txt`.
