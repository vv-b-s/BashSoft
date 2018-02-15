using BashSoft.StaticData;
using BashSoft.StudentRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using BashSoft.SimpleJudge;

namespace BashSoft.IO
{
    public class CommandInterpreter
    {
        private Tester judge;
        private StudentsRepository repository;
        private IOManager IOManager;

        public CommandInterpreter(Tester judge, StudentsRepository repository, IOManager IOManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.IOManager = IOManager;
        }

        /// <summary>
        /// Interprets a command sent form the input. If the command is 'quit' will return 'false'. For any other command returns 'true'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool InterpredCommand(string input)
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
            else if(command == "dropDb")            commandInterpreted = TryDropDatabase(data); 
            else if(command == "show")              commandInterpreted = TryShowWantedData(data);
            else if(command == "help")              commandInterpreted = TryGetHelp(data);
            else if(command == "filter")            commandInterpreted = TryToFilter(data);
            else if(command == "order")             commandInterpreted = TryToOrder(data);
            /*else if(command == "decOrder")          commandInterpreted = TryToOrderDescending(data);
            else if(command == "download")          commandInterpreted = TryToDownload(data);                 NOT IMPLEMENTED
            else if(command == "downloadAsynch")    commandInterpreted = TryToDownloadAsync(data);*/
            else if(command == "quit")              return false;
            else
            {
                OutputWriter.DisplayException(string.Format(ExceptionMessages.InvalidCommandException, command));
                return true;
            }

            if (!commandInterpreted)
                OutputWriter.DisplayException(string.Format(ExceptionMessages.InvalidDataException, string.Join(" ", data)));

            return true;
        }

        #region CLI Methods
        /// <summary>
        /// Opens a file in the current location
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryOpenFile(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var fileName = data.Dequeue();

            //Use current path if it is not absolute
            if (!fileName.Contains(SessionData.PathSeparator))
                fileName = SessionData.CurrentPath + SessionData.PathSeparator + fileName;

            //https://stackoverflow.com/questions/4055266/open-a-file-with-notepad-in-c-sharp
            var process = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    //Tells OS to use the default program
                    UseShellExecute = true,
                    FileName = fileName
                }
            };

            process.Start();

            return true;
        }

        /// <summary>
        /// Create a directory into the current folder
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryCreateDirectory(Queue<string> data)
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
        private bool TryTraverseFolders(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var depthParsed = int.TryParse(data.Dequeue(), out int depth);
            if (depthParsed)
                IOManager.TraverseDirectory(depth);
            else
                OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumberException);

            return true;
        }

        /// <summary>
        /// Compares two files
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryCompareFiles(Queue<string> data)
        {
            if (data.Count != 2)
                return false;

            judge.CompareContent(data.Dequeue(), data.Dequeue());
            return true;
        }

        /// <summary>
        /// Changes the directory to a relative path
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryChangePathRelatively(Queue<string> data)
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
        private bool TryChangePathAbsolute(Queue<string> data)
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
        private bool TryReadDatabaseFromFile(Queue<string> data)
        {
            if (data.Count != 1)
                return false;

            var fileName = data.Dequeue();
            repository.LoadData(fileName);

            return true;
        }

        private bool TryDropDatabase(Queue<string> data)
        {
            if (data.Count != 0)
                return false;

            repository.UnloadData();
            return true;
        }

        /// <summary>
        /// Shows student and marks or students from a course and their marks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryShowWantedData(Queue<string> data)
        {
            var dataCount = data.Count;
            if (dataCount < 1 || dataCount > 2)
                return false;

            var course = data.Dequeue();
            if (dataCount == 1)
                repository.GetAllStudentsFromCourse(course);
            if (dataCount == 2)
            {
                var username = data.Dequeue();
                repository.GetStudentScoresFromCourse(course, username);
            }

            return true;
        }

        /// <summary>
        /// Displays the help messages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryGetHelp(Queue<string> data)
        {
            if (data.Count != 0)
                return false;

            Help.DisplayHelpMessage();
            return true;
        }

        /// <summary>
        /// By the given data, it filters the students
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryToFilter(Queue<string> data) => PerformQueryOperation(data, SortingOperation.Filter);

        /// <summary>
        /// Tries to order the data either by ascedning or descending
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TryToOrder(Queue<string> data) => PerformQueryOperation(data, SortingOperation.Order);

        private bool TryToDownloadAsync(Queue<string> data)
        {
            throw new NotImplementedException();
        }

        private bool TryToDownload(Queue<string> data)
        {
            throw new NotImplementedException();
        }

        private bool TryToOrderDescending(Queue<string> data)
        {
            throw new NotImplementedException();
        } 
        #endregion

        /// <summary>
        /// Either perform filtering or ordering of desired data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private bool PerformQueryOperation(Queue<string> data, SortingOperation operation)
        {
            if (data.Count != 2 && data.Count != 4)
                return false;

            var courseName = data.Dequeue();
            var criteria = data.Dequeue().ToLower();

            //Writing 'take all' is optional
            var takeCommand = "";
            var takeAmountString = "";
            if (data.Count > 0)
            {
                takeCommand = data.Dequeue().ToLower();
                takeAmountString = data.Dequeue().ToLower();
            }

            if (takeCommand != "take" && data.Count > 0)
                return false;

            var dataParsed = int.TryParse(takeAmountString, out int takeNumber);

            if (dataParsed && takeNumber >= 0)
                repository.GetAllStudentsFromCourse(courseName, operation, criteria, takeNumber);

            else if (takeAmountString == "all" || takeAmountString == "")
                repository.GetAllStudentsFromCourse(courseName, operation, criteria);

            else OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumberException);

            return true;
        }
    }
}
