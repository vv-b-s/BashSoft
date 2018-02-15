using System;

using BashSoft.IO;
using BashSoft.StudentRepository;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository.Filtering;

namespace BashSoft
{
    class Launcher
    {
        static void Main(string[] args)
        {
            var tester = new Tester();
            var ioManager = new IOManager();
            var repo = new StudentsRepository(new Sorter(), new Filter());

            var commandInterpreter = new CommandInterpreter(tester, repo, ioManager);
            var reader = new InputReader(commandInterpreter);

            reader.StartReadingCommands();
        }
    }
}
