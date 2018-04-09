using BashSoft.StaticData;
using BashSoft.StudentRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using BashSoft.SimpleJudge;
using BashSoft.Exceptions;
using BashSoft.IO.Commands;
using BashSoft.Contracts;

namespace BashSoft.IO
{
    public class CommandInterpreter:IInterpreter
    {
        private IContentComparer judge;
        private IStudentsRepository repository;
        private IDirectoryManager IOManager;

        public CommandInterpreter(IContentComparer judge, IStudentsRepository repository, IDirectoryManager IOManager)
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
            var data = input.Split();
            var command = data[0];

            if (command == "quit")
                return false;

            try
            {
                var commandObject =  ParseCommand(input, command, data);
                commandObject.Execute();
            }
            catch (Exception ex) { OutputWriter.DisplayException(ex.Message); }

            return true;
        }

        private IExecutable ParseCommand(string input, string command, string[] data)
        {

            if (command == "open")                  return new OpenFileCommand(input, data, judge, repository, IOManager);
            else if(command == "mkdir")             return new MakeDirectoryCommand(input, data, judge, repository, IOManager);
            else if(command == "ls")                return new TraverseFoldersCommand(input, data, judge, repository, IOManager);
            else if(command == "cmp")               return new CompareFilesCommand(input, data, judge, repository, IOManager);
            else if(command == "cdRel")             return new ChangeRelativePathCommand(input, data, judge, repository, IOManager);
            else if(command == "cdAbs")             return new ChangeAbsolutePathCommand(input, data, judge, repository, IOManager);
            else if(command == "readDb")            return new ReadDatabaseCommand(input, data, judge, repository, IOManager);
            else if(command == "dropDb")            return new DropDatabaseCommand(input, data, judge, repository, IOManager);
            else if(command == "show")              return new ShowCourseCommand(input, data, judge, repository, IOManager);
            else if(command == "display")           return new DisplayCommand(input, data, judge, repository, IOManager);          
            else if(command == "help")              return new GetHelpCommand(input, data, judge, repository, IOManager);
            else if(command == "filter")            return new PrintFilteredStudentsCommand(input, data, judge, repository, IOManager);
            else if(command == "order")             return new PrintOrderedStudentsCommand(input, data, judge, repository, IOManager);
            /*else if(command == "decOrder")        commandInterpreted = TryToOrderDescending(data);
            else if(command == "download")          commandInterpreted = TryToDownload(data);                 NOT IMPLEMENTED
            else if(command == "downloadAsynch")    commandInterpreted = TryToDownloadAsync(data);*/

            else                                    throw new InvalidCommandException(command);
        }
    }
}
