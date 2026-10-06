namespace SnapshotKV.Test;

public class UserTests
{
    [Fact]
    public void UserTest1_()
    {

        SnapshotSwitchboard switchBoard = new SnapshotSwitchboard();
        int ID = 100;
        SnapshotTestUtil
            .Start()
            .SetOperation(
                    () => switchBoard.SetEntry(ID,
                                               new int[] { 500, 4, 3, 8, 10, 11, 4, 7, 100, 10, 3, 1 }))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("SetEntry")
                    .SetResults(new int[] { 500, 4, 3, 8, 10, 11, 4, 7, 100, 10, 3, 1 })
                    .SetRecKey(ID)
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(() => switchBoard.Sort(ID))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("Sort")
                    .IgnoreResults()
                    .IgnoreRecKey()
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(() => switchBoard.GetEntry(ID))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("GetEntry")
                    .SetResults(new int[] { 1, 3, 3, 4, 4, 7, 8, 10, 10, 11, 100, 500 })
                    .SetRecKey(ID)
                    .SetSuccess(true)
                    .Build())
            .Next()

            .SetOperation(
                    () => switchBoard.SetEntry(ID,
                                               new int[] { 14, 11, 19, 10, 15, 20, 12, 17, 11, 18, 13, 16, 10, 19, 14 }))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("SetEntry")
                    .SetResults(new int[] { 14, 11, 19, 10, 15, 20, 12, 17, 11, 18, 13, 16, 10, 19, 14 })
                    .SetRecKey(ID)
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(() => switchBoard.Sort(ID))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("Sort")
                    .IgnoreResults()
                    .IgnoreRecKey()
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(() => switchBoard.GetEntry(ID))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("GetEntry")
                    .SetResults(new int[] { 10, 10, 11, 11, 12, 13, 14, 14, 15, 16, 17, 18, 19, 19, 20 })
                    .SetRecKey(ID)
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(
                    () => switchBoard.SetEntry(ID,
                                               new int[] { 2, 1, 3 }))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("SetEntry")
                    .SetResults(new int[] { 2, 1, 3 })
                    .SetRecKey(ID)
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(() => switchBoard.Sort(ID))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("Sort")
                    .IgnoreResults()
                    .IgnoreRecKey()
                    .SetSuccess(true)
                    .Build())
            .Next()
            .SetOperation(() => switchBoard.GetEntry(ID))
            .SetExpected(
                    InternalSnapshotQueryResult.Make()
                    .SetCommand("GetEntry")
                    .SetResults(new int[] { 1, 2, 3 })
                    .SetRecKey(ID)
                    .SetSuccess(true)
                    .Build())
            .Next()
            .Run();


    }





}














