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
        public static void TraverseDirectory(string path)
        {
            WriteEmptyLine();

            var subFolders = new Queue<string>();

            //Getting the initial folder's path into the queue
            subFolders.Enqueue(path);

            //Get the initial depth level to measure correctly the depth later
            var initialDepthLevel = path.Split(@"\").Length;

            //Traverse the subfolders
            while (subFolders.Count > 0)
            { 
                var folderPath = subFolders.Dequeue();

                //Measure the level of depth where the folder is so it can be displayed correctly on the output
                var depthLevel = folderPath.Split(@"\").Length - initialDepthLevel;

                WriteMessageOnNewLine($"{new string('-', depthLevel)}{folderPath}");

                var subFoldersToEnqueue = Directory.GetDirectories(folderPath);
                foreach (var folder in subFoldersToEnqueue)
                    subFolders.Enqueue(folder);
            }
        }
    }
}
