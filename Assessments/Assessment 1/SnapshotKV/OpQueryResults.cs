namespace SnapshotKV;

public class MyQueryResult : SnapshotQueryResult

{
    private string _command;
    private int[] _results;
    private int? _recordedKey;
    private bool _success;

    //public SnapshotKVDB? Database() {return _db; }
    public MyQueryResult(string cmd, int[] results, int? key, bool success)
    {
        _command = cmd;
        _results = results ?? new int[0];
        _recordedKey = key;
        _success = success;



    }
    //Results types were being printed instead of elements. 
    public override string ToString()
    {
        string dataStr = (_results != null) ? string.Join(", ", _results) : "[]";
        return $"Command: {_command} \nResults: [{dataStr}]\nKey: {_recordedKey}\nSuccess: {_success}";
    }


    public string Command()
    {
        return _command;
    }
    public int[] Results()
    {
        return _results;
    }

    public int? RecordedKey()
    {
        return _recordedKey;
    }
    public bool Success()
    {

        return _success;


    }
}



public class OpResults : SnapshotDBOpResult

{
    private string _command;
    private bool _success;
    private SnapshotKVDB? _db;

    public SnapshotKVDB? Database() { return _db; }
    public OpResults(string cmd, bool success, SnapshotKVDB? db = null)
    {
        _command = cmd;
        _success = success;
        _db = db;


    }
    //Results types were being printed instead of elements. 
    public override string ToString()
    {
        //string dataStr = (_results != null) ? string.Join(", ", _results) : "[]";
        return $"Command: {_command} \nSuccess: {_success}";
    }


    public string Command()
    {
        return _command;
    }

    public bool Success()
    {

        return _success;


    }
}
