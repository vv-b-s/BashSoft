using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;
using BashSoft.Contracts;
using BashSoft.Attributes;

namespace BashSoft.IO.Commands
{
    [Alias("cdAbs")]
    public class ChangeAbsolutePathCommand : Command
    {
        public ChangeAbsolutePathCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IDirectoryManager IoManager { get; private set; }

        public override void Execute()
        {
            var absolutePath = this.Data[1];
            IoManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        }
    }
}
