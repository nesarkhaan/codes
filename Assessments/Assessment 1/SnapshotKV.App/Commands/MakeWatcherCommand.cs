namespace SnapshotKV.App;


public class MakeWatcherCommand : SwitchboardResponse
{

    public void ProcessCommand(SnapshotSwitchboard switchboard, string[] args)
    {

        if (args.Length == 0) { Console.WriteLine("Please enter a key, start index and an end index"); return; }


        int key1 = int.Parse(args[0]);
        if (args.Length == 1) { Console.WriteLine("Please enter a key, start index and an end index"); return; }

        int indx1 = int.Parse(args[1]);
        if (args.Length == 2) { Console.WriteLine("Please enter a key, start index and an end index"); return; }

        int indx2 = int.Parse(args[2]);


        var result = switchboard.MakeWatcher(key1, indx1, indx2);

        Console.WriteLine(result);

    }
}
