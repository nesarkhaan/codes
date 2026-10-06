# Mission 4: The Precinct Evidence Locker (Generics)

**The Goal:** Standard Operating Procedures dictate that different types of evidence cannot be mixed. You need to build a secure, generic `EvidenceBox<T>` that can hold exactly one piece of evidence, regardless of whether it is a `Weapon` or a `Document`. 

**Your Tasks:**
1. Review the `Weapon` and `Document` classes (already completed).
2. Complete the generic `EvidenceBox<T>` class:
   - `StoreEvidence(T newItem)`: Places the item in the box. If the box is already sealed, return `false`. If successful, return `true`.
   - `SealBox()`: Sets the `IsSealed` boolean to true, meaning the evidence can no longer be tampered with.
   - `ExamineEvidence()`: Returns the item `T` currently inside the box. If the box is empty, it should return the default empty value for that object.
