using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StaticData;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    [Alias("ls")]
    public class TraverseFoldersCommand : Command
    {
        public TraverseFoldersCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IDirectoryManager IoManager { get; private set; }

        public override void Execute()
        {
            var depthParsed = int.TryParse(this.Data[1], out int depth);

            if (depthParsed)
                IoManager.TraverseDirectory(depth);

            else
                throw new FormatException(ExceptionMessages.UnableToParseNumberException);
        }
    }
}
