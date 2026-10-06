namespace EvidenceManager;

// The <T> here tells C# "This class works with ANY type, and I'll call that type T"
public class EvidenceBox<T>
{
    private T storedItem;
    private bool isSealed;
    private bool isEmpty;

    public EvidenceBox()
    {
        // When a box is first created, it is empty and unsealed.
        isEmpty = true;
        isSealed = false;
        
        // This is how we safely set a generic variable to "nothing" to start
        storedItem = default(T); 
    }

    public bool StoreEvidence(T newItem)
    {
        // TODO: Check if the box is sealed. If it is, return false (cannot store!).
        // TODO: If not sealed, assign newItem to storedItem, set isEmpty to false, and return true.
        return false;
    }

    public void SealBox()
    {
        // TODO: Lock it down.
    }

    public T ExamineEvidence()
    {
        // TODO: If the box isEmpty, return default(T);
        // TODO: Otherwise, return the storedItem.
        return default(T);
    }
}
