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
    [Alias("readDb")]
    public class ReadDatabaseCommand : Command
    {
        public ReadDatabaseCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IStudentsRepository Repository { get; private set; }

        public override void Execute()
        {
            var fileName = this.Data[1];
            Repository.LoadData(fileName);
        }
    }
}
