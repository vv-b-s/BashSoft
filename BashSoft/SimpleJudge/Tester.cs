using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BashSoft.IO;
using BashSoft.StaticData;

namespace BashSoft.SimpleJudge
{
    public class Tester
    {
        /// <summary>
        /// Compare two files, display and save their mismatches
        /// </summary>
        /// <param name="userOutputPath"></param>
        /// <param name="expectedOutputPath"></param>
        public static void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");

            //Convert relative paths to absolute
            if (!userOutputPath.Contains(SessionData.PathSeparator))
                userOutputPath = SessionData.CurrentPath + SessionData.PathSeparator + userOutputPath;
            if (!expectedOutputPath.Contains(SessionData.PathSeparator))
                expectedOutputPath = SessionData.CurrentPath + SessionData.PathSeparator + expectedOutputPath;

            //It is a good practice to avoid try-catch if possible
            if (File.Exists(userOutputPath) && File.Exists(expectedOutputPath))
            {
                var mismatchPath = GetMismatchPath(expectedOutputPath);

                //Read both files
                var actualOutputLines = File.ReadAllLines(userOutputPath);
                var expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                var mismatches = GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputLines, out bool hasMismatch);

                PrintOutput(mismatches, hasMismatch, mismatchPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            else OutputWriter.DisplayException(ExceptionMessages.InvalidPathException);

        }

        /// <summary>
        /// Will print all the found mismathces if any and will save them in a separate .txt file
        /// </summary>
        /// <param name="mismatches"></param>
        /// <param name="hasMismatch"></param>
        /// <param name="mismatchPath"></param>
        private static void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if(hasMismatch)
            {
                var outputString = new StringBuilder();
                foreach (var line in mismatches)
                    outputString.AppendLine(line);

                OutputWriter.WriteMessage(outputString.ToString());

                //Check if the folder is valid
                var folderPath = mismatchPath.Substring(0, mismatchPath.LastIndexOf(SessionData.PathSeparator));
                if (Directory.Exists(folderPath))
                        File.WriteAllText(mismatchPath, outputString.ToString());
                    else OutputWriter.DisplayException(ExceptionMessages.InvalidPathException);

                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }

        /// <summary>
        /// Creates the path for the output Mismatch.txt file
        /// </summary>
        /// <param name="expectedOutputPath"></param>
        /// <returns></returns>
        private static string GetMismatchPath(string expectedOutputPath)
        {
            var pathSeparator = SessionData.PathSeparator;

            //Get the folder where the output will be
            var lastIndexOfPathSeparator = expectedOutputPath.LastIndexOf(pathSeparator);

            // If path C:\Folder\file.txt, directoryPath is: C:\Folder
            var directoryPath = expectedOutputPath.Substring(0, lastIndexOfPathSeparator);

            //filePath = "\Mismatches.txt"
            var filePath = directoryPath + $@"{pathSeparator}Mismatches.txt";

            return filePath;
        }

        /// <summary>
        /// Compares both files line by line and points out the mismatches
        /// </summary>
        /// <param name="userLines"></param>
        /// <param name="expectedLines"></param>
        /// <param name="hasMismatch"></param>
        /// <returns></returns>
        private static string[] GetLinesWithPossibleMismatches(string[] userLines, string[] expectedLines, out bool hasMismatch)
        {
            var minLines = expectedLines.Length;
            hasMismatch = false;

            //Compare the line size to escape the probability of out of range exceptions
            if (userLines.Length != expectedLines.Length)
            {
                minLines = Math.Min(userLines.Length, expectedLines.Length);
                OutputWriter.DisplayException(ExceptionMessages.ComparisonOfFilesWithDifferentSizesException);
                hasMismatch = true;
            }

            var mismatches = new string[minLines];

            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            for (int i = 0; i < minLines; i++)
            {
                if (userLines[i] != expectedLines[i])
                {
                    var text = $"Mismatch at line {i} -- expected: \"{expectedLines[i]}\", actual: \"{userLines[i]}\"" + Environment.NewLine;
                    mismatches[i] = text;
                    hasMismatch = true;
                }
                else
                    mismatches[i] = userLines[i] + Environment.NewLine;
            }

            return mismatches;
        }
    }
}
