using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StaticData;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class TraverseFoldersCommand : Command
    {
        public TraverseFoldersCommand(string input, string[] data, IContentComparer judge, IStudentsRepository repository, IDirectoryManager iOManager) : base(input, data, judge, repository, iOManager)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            var depthParsed = int.TryParse(this.Data[1], out int depth);

            if (depthParsed)
                InputOutputManager.TraverseDirectory(depth);

            else
                throw new FormatException(ExceptionMessages.UnableToParseNumberException);
        }
    }
}
