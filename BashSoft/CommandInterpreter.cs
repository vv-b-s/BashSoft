using BashSoft.IO;
using BashSoft.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using static BashSoft.IO.OutputWriter;

namespace BashSoft
{
    public static class CommandInterpreter
    {
        /// <summary>
        /// Interprets a command sent form the input. If the command is 'quit' will return 'false'. For any other command returns 'true'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool InterpredCommand(string input)
        {
            var data = new Queue<string>(input.Split());
            var command = data.Dequeue();

            var commandInterpreted = false;

            if (command == "open")                  commandInterpreted = TryOpenFile(data);
            else if(command == "mkdir")             commandInterpreted = TryCreateDirectory(data);
            else if(command == "ls")                commandInterpreted = TryTraverseFolders(data);
            else if(command == "cmp")               commandInterpreted = TryCompareFiles(data);
            else if(command == "cdRel")             commandInterpreted = TryChangePathRelatively(data);
            else if(command == "cdAbs")             commandInterpreted = TryChangePathAbsolute(data);
            else if(command == "readDb")            commandInterpreted = TryReadDatabaseFromFile(data);
            else if(command== "show")               commandInterpreted = TryShowWantedData(data);
            else if(command == "help")              commandInterpreted = TryGetHelp(data);
            else if(command == "filter")            commandInterpreted = TryToFilter(data);
            else if(command == "order")             commandInterpreted = TryToOrder(data);
            else if(command == "decOrder")          commandInterpreted = TryToOrderDescending(data);
            else if(command == "download")          commandInterpreted = TryToDownload(data);
            else if(command == "downloadAsynch")    commandInterpreted = TryToDownloadAsync(data);
            else if(command == "quit")              return false;
            else
            {
                DisplayException($"Invalid command: {command}");
                return true;
            }

            if (!commandInterpreted)
                DisplayException($"Invalid data: {string.Join(" ", data)}");

            return true;
        }

        /// <summary>
        /// Opens a file in the current location
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryOpenFile(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var fileName = SessionData.CurrentPath + SessionData.PathSeparator + data.Dequeue();
            Process.Start(fileName);

            return true;
        }
        
        /// <summary>
        /// Create a directory into the current folder
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryCreateDirectory(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var folderName = data.Dequeue();
            IOManager.CreateDirectoryInCurrentFolder(folderName);
            return true;
        }

        /// <summary>
        /// Invokes traversion
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryTraverseFolders(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var depthParsed = int.TryParse(data.Dequeue(), out int depth);
            if (depthParsed)
                IOManager.TraverseDirectory(depth);
            else
                DisplayException(ExceptionMessages.UnableToParseNumberException);

            return true;
        }

        /// <summary>
        /// Compares two files
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryCompareFiles(Queue<string> data)
        {
            if (data.Count != 2)
                return false;

            SimpleJudge.Tester.CompareContent(data.Dequeue(), data.Dequeue());
            return true;
        }

        /// <summary>
        /// Changes the directory to a relative path
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryChangePathRelatively(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var relPath = data.Dequeue();
            IOManager.ChangeCurrentDirectoryRelative(relPath);

            return true;
        }

        /// <summary>
        /// Goes to an absolute path if possible
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryChangePathAbsolute(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var absolutePath = data.Dequeue();
            IOManager.ChangeCurrentDirectoryAbsolute(absolutePath);
            return true;
        }

        /// <summary>
        /// Reads the database of students, courses and marks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryReadDatabaseFromFile(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var fileName = data.Dequeue();
            StudentsRepository.InitializeData(fileName);

            return true;
        }

        /// <summary>
        /// Shows student and marks or students from a course and their marks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryShowWantedData(Queue<string> data)
        {
            var dataCount = data.Count;
            if (dataCount < 1 || dataCount > 2)
                return false;

            var course = data.Dequeue();
            if (dataCount == 1)
                StudentsRepository.GetAllStudentsFromCourse(course);                
            if (dataCount == 2)
            {
                var username = data.Dequeue();
                StudentsRepository.GetStudentScoresFromCourse(course, username);
            }

            return true;
        }

        /// <summary>
        /// Displays the help messages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool TryGetHelp(Queue<string> data)
        {
            if (data.Count != 0)
                return false;

            Help.DisplayHelpMessage();
            return true;
        }

        private static bool TryToDownloadAsync(Queue<string> data)
        {
            throw new NotImplementedException();
        }

        private static bool TryToDownload(Queue<string> data)
        {
            throw new NotImplementedException();
        }

        private static bool TryToOrderDescending(Queue<string> data)
        {
            throw new NotImplementedException();
        }

        private static bool TryToOrder(Queue<string> data)
        {
            throw new NotImplementedException();
        }

        private static bool TryToFilter(Queue<string> data)
        {
            throw new NotImplementedException();
        }
    }
}
