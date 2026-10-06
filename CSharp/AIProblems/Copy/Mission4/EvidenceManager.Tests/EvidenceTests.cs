using Xunit;
using EvidenceManager;

namespace EvidenceManager.Tests;

public class EvidenceTests
{
    [Fact]
    public void Box_CanStoreAndRetrieve_Weapon()
    {
        // Notice how we tell the box exactly what it will hold using <Weapon>
        EvidenceBox<Weapon> gunBox = new EvidenceBox<Weapon>();
        Weapon glock = new Weapon("GLK-9921");

        bool stored = gunBox.StoreEvidence(glock);
        Weapon retrieved = gunBox.ExamineEvidence();

        Assert.True(stored);
        Assert.Equal("GLK-9921", retrieved.GetSerialNumber());
    }

    [Fact]
    public void Box_CanStoreAndRetrieve_Document()
    {
        // Now we use the EXACT SAME CLASS, but tell it to be a Document box!
        EvidenceBox<Document> fileBox = new EvidenceBox<Document>();
        Document statement = new Document("CASE-2026-A");

        fileBox.StoreEvidence(statement);
        Document retrieved = fileBox.ExamineEvidence();

        Assert.Equal("CASE-2026-A", retrieved.GetCaseFileCode());
    }

    [Fact]
    public void SealedBox_RejectsNewEvidence()
    {
        EvidenceBox<Weapon> secureBox = new EvidenceBox<Weapon>();
        secureBox.SealBox();

        Weapon rifle = new Weapon("AR-100");
        bool result = secureBox.StoreEvidence(rifle);

        Assert.False(result); // Should fail because the box is sealed
    }
}
