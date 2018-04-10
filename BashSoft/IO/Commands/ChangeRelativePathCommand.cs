using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    [Alias("cdRel")]
    public class ChangeRelativePathCommand : Command
    {
        public ChangeRelativePathCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IDirectoryManager IoManager { get; private set; }

        public override void Execute()
        {
            var relPath = this.Data[1];
            IoManager.ChangeCurrentDirectoryRelative(relPath);
        }
    }
}
