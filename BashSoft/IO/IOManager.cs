using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


using static BashSoft.IO.OutputWriter;

namespace BashSoft.IO
{
    public static class IOManager
    {
        /// <summary>
        /// Use Breadth First Search Algorithm to display all the directories and subdirectories in a given path
        /// https://upload.wikimedia.org/wikipedia/commons/5/5d/Breadth-First-Search-Algorithm.gif
        /// </summary>
        /// <param name="path"></param>
        public static void TraverseDirectory(int endDepth)
        {
            var path = SessionData.CurrentPath;
            WriteEmptyLine();

            var subFolders = new Queue<string>();

            //Getting the initial folder's path into the queue
            subFolders.Enqueue(path);

            //Get the initial depth level to measure correctly the depth later
            var pathSeparator = SessionData.PathSeparator;
            var initialDepthLevel = path.Split(pathSeparator).Length;

            //Using StringBuilder to display tree faster
            var filesAndFolders = new StringBuilder();

            //Traverse the subfolders
            while (subFolders.Count > 0)
            { 
                var folderPath = subFolders.Dequeue();

                //Measure the level of depth where the folder is so it can be displayed correctly on the output
                var currentDepth = folderPath.Split(pathSeparator).Length - initialDepthLevel;

                //This means that we needn't traverse any further
                if (endDepth - currentDepth < 0)
                    break;

                filesAndFolders.AppendLine($"{new string('-', currentDepth)}{folderPath}");

                //Print the files in the folder
                var files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    var lastSlash = file.LastIndexOf(pathSeparator);

                    //Getting only the file name
                    var fileName = file.Substring(lastSlash);

                    //Full path will be replaced with dashes, to point out files are located there
                    filesAndFolders.AppendLine($"{new string('-', lastSlash)}{fileName}");
                }

                //Enqueue the subfolders
                var subFoldersToEnqueue = Directory.GetDirectories(folderPath);
                foreach (var folder in subFoldersToEnqueue)
                    subFolders.Enqueue(folder);
            }

            WriteMessageOnNewLine(filesAndFolders.ToString());
        }

        /// <summary>
        /// Creates a directory in the curent folder the program is in
        /// </summary>
        /// <param name="newDirectoryName"></param>
        public static void CreateDirectoryInCurrentFolder(string newDirectoryName)
        {
            var pathSeparator = SessionData.PathSeparator;

            //E.g. C:\curentPathFolder + "\newDirectoryName"
            var newPath = SessionData.CurrentPath + pathSeparator + newDirectoryName;
            Directory.CreateDirectory(newPath);
        }
    }
}
