namespace FileShare.Lib;


// This is used to report progress to the user.
public record PackageState(
    string Identifier, 
    string Filename, 
    bool IsComplete, 
    double ProgressPercentage
);
