using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class DropDatabaseCommand : Command
    {
        public DropDatabaseCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager iOManager) : base(input, data, judge, repository, iOManager)
        {
            if (data.Length != 1)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            Repository.UnloadData();
        }
    }
}
