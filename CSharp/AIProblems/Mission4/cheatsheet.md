# Cheat Sheet: Working with Generics <T>

**1. What is T?**
`T` is just a placeholder. When you type `new EvidenceBox<Weapon>()`, the compiler goes into your class and literally replaces every single `T` it sees with the word `Weapon`.

**2. Defining a Generic Field:**
Just like you can have a `private string name;`, you can have a field typed as `T`:
`private T storedItem;`

**3. Returning T:**
If a method needs to hand the item back, its return type is simply `T`.
```csharp
public T GetItem()
{
    return storedItem;
}
