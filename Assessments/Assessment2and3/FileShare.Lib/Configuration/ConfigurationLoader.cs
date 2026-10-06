using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileShare.Lib.Configuration
{
    public static class ConfigurationLoader
    {
        public static FileShareConfig Load(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(
                    $"Configuration file not found: {configPath}");
            }

            var config = new FileShareConfig();

            Dictionary<string, string> values =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (string line in File.ReadAllLines(configPath))
            {
                string text = line.Trim();

                // Ignore blank lines
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                // Ignore comments
                if (text.StartsWith("#"))
                    continue;
                // Ignore lines that don't contain an '=' character
                int index = text.IndexOf('=');
                // Ignore lines that don't contain an '=' character
                if (index < 0)
                    continue;
                // Ignore lines with empty keys or values
                string key = text.Substring(0, index).Trim();               
                string value = text.Substring(index + 1).Trim();
                
                values[key] = value;
            }
            // Validate that all required fields are present
            ValidateRequiredFields(values);

            config.Nickname = values["nickname"];
            config.MetaFileDirectory = values["metafile_directory"];
            config.FileDirectory = values["file_directory"];
            config.MaxPeers = int.Parse(values["max_peers"]);
            config.Port = int.Parse(values["port"]);

            Validate(config);

            return config;
        }
        // Validate that all required fields are present in the configuration file
        private static void ValidateRequiredFields(
            Dictionary<string, string> values)
        {
            string[] required =
            {
                "nickname",
                "metafile_directory",
                "file_directory",
                "max_peers",
                "port"
            };
            // Check for missing required fields
            foreach (string key in required)
            {
                if (!values.ContainsKey(key))
                {
                    throw new Exception(
                        $"Missing configuration value: {key}");
                }
            }
        }
        // Validate the configuration values for correctness
        private static void Validate(FileShareConfig config)
        {   // Validate that max_peers is within the valid range
            if (config.MaxPeers < 1 || config.MaxPeers > 2048)
            {
                throw new Exception(
                    "max_peers within the configuration file is set to an invalid value");
            }
            //  
            if (config.Port < 1 || config.Port > 65535)
            {
                throw new Exception(
                    "Port specified is either in use or invalid");
            }
            // Validate that the metafile_directory and file_directory are valid directories
            CreateDirectory(config.MetaFileDirectory,
                "Unable to create metafile_directory");
            // Validate that the file_directory is a valid directory
            CreateDirectory(config.FileDirectory,
                "Unable to create file_directory");
        }
        // Create a directory if it does not exist, and throw an exception if a file with the same name exists
        private static void CreateDirectory(string directory, string errorMessage)
        {
            // Check if a file with the same name exists
            if (File.Exists(directory))
            {
                throw new Exception(errorMessage);
            }
            // Create the directory if it does not exist
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
