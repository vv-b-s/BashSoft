using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static BashSoft.IO.OutputWriter;
using BashSoft.StaticData;

namespace BashSoft.IO
{
    public static class IOManager
    {
        /// <summary>
        /// Use Breadth First Search Algorithm to display all the directories and subdirectories in a given path
        /// https://upload.wikimedia.org/wikipedia/commons/5/5d/Breadth-First-Search-Algorithm.gif
        /// </summary>
        /// <param name="endDepth"></param>
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

            //Traverse the subfolders
            while (subFolders.Count > 0)
            {

                var folderPath = subFolders.Dequeue();

                //Measure the level of depth where the folder is so it can be displayed correctly on the output
                var currentDepth = folderPath.Split(pathSeparator).Length - initialDepthLevel;

                //This means that we needn't traverse any further
                if (endDepth - currentDepth < 0)
                    break;

                WriteMessageOnNewLine($"{new string('-', currentDepth)}{folderPath}");

                try
                {
                    //Print the files in the folder
                    var files = Directory.GetFiles(folderPath);
                    foreach (var file in files)
                    {
                        var lastSlash = file.LastIndexOf(pathSeparator);

                        //Getting only the file name
                        var fileName = file.Substring(lastSlash);

                        //Full path will be replaced with dashes, to point out files are located there
                        WriteMessageOnNewLine($"{new string('-', lastSlash)}{fileName}");
                    }

                    //Enqueue the subfolders
                    var subFoldersToEnqueue = Directory.GetDirectories(folderPath);
                    foreach (var folder in subFoldersToEnqueue)
                        subFolders.Enqueue(folder);
                }
                catch (UnauthorizedAccessException) { DisplayException(ExceptionMessages.AccessDeniedException); }
            }
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

            try { Directory.CreateDirectory(newPath); }
            catch (ArgumentException) { DisplayException(ExceptionMessages.ForbiddenSymbolsContainedInNameException); }
        }

        /// <summary>
        /// Change the directory to a relative folder. Use '..' to go UP and [folder name] to go inside a folder
        /// </summary>
        /// <param name="relativePath"></param>
        public static void ChangeCurrentDirectoryRelative(string relativePath)
        {
            var currentPath = SessionData.CurrentPath;
            
            //Go one folder up
            if(relativePath == "..")
            {
                try
                {
                    //If the current path is 'C:\Users\Folder' we want to make it 'C:\Users\'
                    var indexOfLastPathSeparator = currentPath.LastIndexOf(SessionData.PathSeparator);
                    var newPath = currentPath.Substring(0, indexOfLastPathSeparator);

                    SessionData.CurrentPath = newPath;
                }
                catch (ArgumentOutOfRangeException) { DisplayException(ExceptionMessages.InvalidUPOperationException); }
            }

            //Otherwsise if we want to enter a folder
            else
            {
                //If the last symbol is a slash than we needn't add anothr one below in the code
                var lastSymbolIsSeparator = currentPath[currentPath.Length - 1].ToString() == SessionData.PathSeparator;

                //If current folder is 'C:\Users\Folder' and we want to enter SubFoldEr then the resulting path would be 'C:\Users\Folder\SubFoldEr'
                if(lastSymbolIsSeparator)
                    currentPath += relativePath;
                else
                    currentPath += $"{SessionData.PathSeparator}{relativePath}";

                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public static void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if(Directory.Exists(absolutePath))
            {
                SessionData.CurrentPath = absolutePath;
                return;
            }

            //If the path does not exist, will display exception
            DisplayException(ExceptionMessages.InvalidPathException);
        }
    }
}
