using Xunit;
using Precinct;

namespace Precinct.Tests;

public class PrecinctTests
{
    [Fact]
    public void BaseOfficer_PerformsStandardDuty()
    {
        PoliceOfficer officer = new PoliceOfficer("Murphy", "104");
        Assert.Equal("Murphy is filling out standard precinct paperwork.", officer.PerformDuty());
        Assert.Equal("104", officer.GetBadge());
    }

    [Fact]
    public void PatrolOfficer_OverridesDuty_WithZone()
    {
        // Notice we can store a PatrolOfficer in a PoliceOfficer variable! That is the power of inheritance.
        PoliceOfficer patrol = new PatrolOfficer("Smith", "775", "Downtown");
        Assert.Equal("Smith is actively patrolling zone Downtown.", patrol.PerformDuty());
    }

    [Fact]
    public void Detective_OverridesDuty_BasedOnCoverStatus()
    {
        Detective undercoverDet = new Detective("Diaz", "99", true);
        Detective standardDet = new Detective("Peralta", "95", false);

        Assert.Equal("Diaz is gathering evidence in secret.", undercoverDet.PerformDuty());
        Assert.Equal("Peralta is interviewing witnesses.", standardDet.PerformDuty());
    }
}
