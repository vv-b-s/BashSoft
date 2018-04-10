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
    [Alias("dropDb")]
    public class DropDatabaseCommand : Command
    {
        public DropDatabaseCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 1)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IStudentsRepository Repository { get; private set; }

        public override void Execute()
        {
            Repository.UnloadData();
        }
    }
}
