# Cheat Sheet: Working with List<T>

**1. Creating and Adding to a List:**
Unlike arrays where you do `arr[i] = item`, Lists have built-in methods.
`myList.Add(newItem);`

**2. Checking the Size:**
Arrays use `.Length`. Lists use `.Count`.
`int size = myList.Count;`

**3. Removing an Item:**
Lists do the "shifting" math for you! If you find the object you want to remove, you just call:
`myList.Remove(itemToRemove);`

**4. The DropOut Logic Hint:**
You can use a `foreach` loop or a `for` loop to find the student. 
```csharp
foreach (Student s in Roster) 
{
    if (s.GetName() == targetName) 
    {
        Roster.Remove(s);
        return true;
    }
}
