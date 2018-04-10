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
    [Alias("cmp")]
    public class CompareFilesCommand : Command
    {
        public CompareFilesCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 3)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IContentComparer Judge { get; private set; }

        public override void Execute()
        {
            Judge.CompareContent(userOutputPath: this.Data[1], expectedOutputPath: this.Data[2]);
        }
    }
}
