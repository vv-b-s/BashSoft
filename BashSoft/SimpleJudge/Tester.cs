using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BashSoft.IO;

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

            var mismatchPath = GetMismatchPath(expectedOutputPath);

            //Read both files
            var actualOutputLines = File.ReadAllLines(userOutputPath);
            var expectedOutputLines = File.ReadAllLines(expectedOutputPath);

            var mismatches = GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputLines, out bool hasMismatch);

            PrintOutput(mismatches, hasMismatch, mismatchPath);
            OutputWriter.WriteMessageOnNewLine("Files read!");
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
                foreach (var line in mismatches)
                    OutputWriter.WriteMessageOnNewLine(line);

                File.WriteAllLines(mismatchPath, mismatches);
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
            var mismatches = new string[userLines.Length];
            hasMismatch = false;

            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            for (int i = 0; i < userLines.Length; i++)
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
