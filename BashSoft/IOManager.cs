using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


using static BashSoft.OutputWriter;

namespace BashSoft
{
    public static class IOManager
    {
        public static void TraverseDirectory(string path)
        {
            WriteEmptyLine();

            var subFolders = new Queue<string>();

            //The folder coresponding to the path is the deepest one e.g. C:\Users\Documents\SpecialDocuments\LessSpecialSpecialDocuments\Fffffolderrrr
            subFolders.Enqueue(path);

            //Traverse the subfolders
            while (subFolders.Count > 0)
            { 
                var folderPath = subFolders.Dequeue();

                //Measure the level of depth where the folder is so it can be displayed correctly on the output
                var depthLevel = folderPath.Split(@"\").Length;

                WriteMessageOnNewLine($"{new string('-', depthLevel)}{folderPath}");

                var subFoldersToEnqueue = Directory.GetDirectories(folderPath);
                foreach (var folder in subFoldersToEnqueue)
                    subFolders.Enqueue(folder);
            }
        }
    }
}
