using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StaticData;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    /// <summary>
    /// This class is used to share almost the same logic between Sorting and Filtering commands
    /// </summary>
    public abstract class ModifiablePrintCommand : Command
    {
        public ModifiablePrintCommand(string input, string[] data) : base(input, data)
        {
            if (data.Length != 3 && data.Length != 5)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IStudentsRepository Repository { get; private set; }

        public abstract SortingOperation SortingOperation { get; }

        public override void Execute()
        {
            var data = new Queue<string>(this.Data.Skip(1));

            var courseName = data.Dequeue();
            var criteria = data.Dequeue().ToLower();

            //Writing 'take all' is optional
            var takeCommand = "";
            var takeAmountString = "";
            if (data.Count > 0)
            {
                takeCommand = data.Dequeue().ToLower();
                takeAmountString = data.Dequeue().ToLower();
            }

            if (takeCommand != "take" && data.Count > 0)
                throw new InvalidCommandException(this.Input);

            var dataParsed = int.TryParse(takeAmountString, out int takeNumber);

            if (dataParsed && takeNumber >= 0)
                Repository.GetAllStudentsFromCourse(courseName, SortingOperation, criteria, takeNumber);

            else if (takeAmountString == "all" || takeAmountString == "")
                Repository.GetAllStudentsFromCourse(courseName, SortingOperation, criteria);

            else throw new FormatException(ExceptionMessages.UnableToParseNumberException);
        }
    }
}
