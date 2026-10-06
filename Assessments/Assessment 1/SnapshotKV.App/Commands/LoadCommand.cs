namespace SnapshotKV.App;


public class LoadCommand : SwitchboardResponse
{

    public void ProcessCommand(SnapshotSwitchboard switchboard, string[] args)
    {
        if (args.Length == 0) { Console.WriteLine("Please enter filename: "); return; }
        string path = args[0];
        var result = switchboard.Load(path);

        Console.WriteLine(result);
    }
}
