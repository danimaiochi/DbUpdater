using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DbUpdater.Helpers
{
    public static class FilesHelper
    {
        public static List<(string filePath, Version version)> GetPendingUpdateFiles(Version currentVersion)
        {
            var files = new List<(string filePath, Version version)>();
            var directories = Directory.GetDirectories("Scripts/");

            foreach (var directory in directories)
            {
                int major;
                if (int.TryParse(Path.GetFileName(directory), out major))
                {
                    if (major < currentVersion.Major)
                    {
                        continue;
                    }
                    else if (major == currentVersion.Major)
                    {
                        var filesInCurrentMajor = Directory.GetFiles(directory);

                        foreach (var fileInCurrentMajor in filesInCurrentMajor)
                        {
                            var fileVersion = GetVersionFromFile(fileInCurrentMajor);

                            if (fileVersion != null && fileVersion > currentVersion)
                            {
                                files.Add((fileInCurrentMajor, fileVersion));
                            }
                        }
                    }
                    else if (major > currentVersion.Major)
                    {
                        var filesInCurrentMajor = Directory.GetFiles(directory);

                        foreach (var fileInCurrentMajor in filesInCurrentMajor)
                        {
                            var fileVersion = GetVersionFromFile(fileInCurrentMajor);

                            files.Add((fileInCurrentMajor, fileVersion));
                        }
                    }
                }
            }

            return files.OrderBy(x => x.version).ToList();
        }

        public static Version GetVersionFromFile(string filePath)
        {
            var directory = Path.GetFileName(Path.GetDirectoryName(filePath));
            var fileName = Path.GetFileName(filePath);
            var underscorePosition = fileName.IndexOf('_');

            if (underscorePosition <= 0)
            {
                throw new Exception($"Invalid file found: {filePath}." +
                                    $"\nFiles must start with a number followed by an underscore." +
                                    $"\nFor example: 01_CreatingTableX.sql");
            }

            fileName = fileName.Substring(0, underscorePosition);

            int major;
            int minor;
            if (!int.TryParse(directory, out major) || !int.TryParse(fileName, out minor))
            {
                throw new Exception($"Invalid file found: {filePath}." +
                                    $"\nFiles must be inside of a folder that represents the current year and" +
                                    $"\nstart with a number followed by an underscore." +
                                    $"\nFor example: 2021\\01_CreatingTableX.sql");
            }

            return new Version($"{major}.{minor}");
        }
    }
}