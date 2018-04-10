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
    [Alias("help")]
    public class GetHelpCommand : Command
    {
        public GetHelpCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 1)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            Help.DisplayHelpMessage();
        }
    }
}
