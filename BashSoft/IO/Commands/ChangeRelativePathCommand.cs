using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class ChangeRelativePathCommand : Command
    {
        public ChangeRelativePathCommand(string input, string[] data, IContentComparer judge, IStudentsRepository repository, IDirectoryManager iOManager) : base(input, data, judge, repository, iOManager)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            var relPath = this.Data[1];
            InputOutputManager.ChangeCurrentDirectoryRelative(relPath);
        }
    }
}
