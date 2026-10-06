namespace FileShare.Abstractions.Interfaces;


// Adapter interface for validating and opening a package based on its metadata.

public interface IFileValidationAdapter : IMetadataReader
{

    // Opens a managed package based on the provided descriptor path and data directory.

    IManagedPackage OpenPackage(string descriptorPath, string dataDirectory);
}
