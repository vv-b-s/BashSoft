using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;
using BashSoft.Contracts;

namespace BashSoft.IO.Commands
{
    public class ChangeAbsolutePathCommand : Command
    {
        public ChangeAbsolutePathCommand(string input, string[] data, IContentComparer judge, IStudentsRepository repository, IDirectoryManager iOManager) : base(input, data, judge, repository, iOManager)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            var absolutePath = this.Data[1];
            InputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        }
    }
}
