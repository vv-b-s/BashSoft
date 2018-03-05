using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class CompareFilesCommand : Command
    {
        public CompareFilesCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager iOManager) : base(input, data, judge, repository, iOManager)
        {
            if (data.Length != 3)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            Judge.CompareContent(userOutputPath: this.Data[1], expectedOutputPath: this.Data[2]);
        }
    }
}
