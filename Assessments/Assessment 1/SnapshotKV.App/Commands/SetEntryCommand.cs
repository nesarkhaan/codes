namespace SnapshotKV.App;


public class SetEntryCommand : SwitchboardResponse
{
	
    public void ProcessCommand(SnapshotSwitchboard switchboard, string[] args)
	{
        // int key1 = int.Parse(args[0]);
        //
        if(!int.TryParse(args[0], out int key1)) {Console.WriteLine("Error: The Key must be a valid integer."); return;}

        if(args.Length < 2){
            Console.WriteLine("Error: Please Enter a key and a value. For Example ListKeys 1 2 3 4 5 -- 1 is key, 2 3 4 5 are values");
        
        } else {
        int[] values = new int[args.Length-1];
        for(int i = 0; i < values.Length; i++)
        {
            values[i] = int.Parse(args[i+1]);
        }
        var result = switchboard.SetEntry(key1, values);
		
        Console.WriteLine(result);
        }	
	}
}
