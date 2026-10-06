# Cheat Sheet: String Parsing & Object State

**1. Skipping Empty Lines:**
Your text file has blank lines to make it readable. If your loop tries to parse a blank line, it will crash. Put this at the very top of your `foreach` loop:
```csharp
if (string.IsNullOrWhiteSpace(line)) { continue; }



. Checking the Prefix:
To figure out what kind of data the line holds, use the .StartsWith() method:

C#
if (line.StartsWith("Q: ")) { ... }
else if (line.StartsWith("A: ")) { ... }


3. Chopping off the Prefix (Substring):
You don't want the actual "Q: " or "A: " characters saved in your object. Use .Substring(3) to tell C# to chop off the first 3 characters and keep the rest:
string cleanText = line.Substring(3);

4. The "Current Object" Pointer Trap:
When you are reading an "A:" line, how do you know which question it belongs to? You have to keep a variable outside the loop that remembers the last question you created.

C#
PressQuestion currentQuestion = null;

foreach(string line in lines) 
{
    if (line.StartsWith("Q: ")) 
    {
        currentQuestion = new PressQuestion(cleanText);
        deck.Add(currentQuestion);
    }
    else if (line.StartsWith("A: ")) 
    {
        // Because currentQuestion remembers the last Q: we saw, we can just add to it!
        currentQuestion.AddAnswer(cleanText); 
    }
}
5. Picking a Random Item from a List:

C#
Random rand = new Random();
int randomIndex = rand.Next(myList.Count);
string randomAnswer = myList[randomIndex];

---

Now your documentation perfectly matches the heavy-duty parsing logic required for that massive dataset you just built. 

**Would you like to try writing out the `BriefingManager.cs` class with this new l
