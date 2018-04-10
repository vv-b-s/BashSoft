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
    [Alias("mkdir")]
    public class MakeDirectoryCommand : Command
    {
        public MakeDirectoryCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IDirectoryManager IoManager { get; private set; }

        public override void Execute()
        {
            var folderName = this.Data[1];
            IoManager.CreateDirectoryInCurrentFolder(folderName);
        }
    }
}
