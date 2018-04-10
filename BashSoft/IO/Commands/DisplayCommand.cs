using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("display")]
    public class DisplayCommand : Command
    {
        public DisplayCommand(string input, string[] data) : base(input, data)
        {
            if (this.Data.Length != 3)
                throw new InvalidCommandException(this.Input);
        }

        [Inject]
        public IStudentsRepository Repository { get; private set; }

        public override void Execute()
        {
            var entityToDisplay = this.Data[1];
            var sortType = this.Data[2];

            if (entityToDisplay.Equals("students", StringComparison.OrdinalIgnoreCase))
            {
                var studentComparator = this.CreateComparator<IStudent>(sortType);
                var list = this.Repository.GetAllStudentsSorted(studentComparator);
                OutputWriter.WriteMessageOnNewLine(list.JoinWth(Environment.NewLine));
            }

            else if (entityToDisplay.Equals("courses", StringComparison.OrdinalIgnoreCase))
            {
                var courseCompartor = this.CreateComparator<ICourse>(sortType);
                var list = this.Repository.GetAlCoursesSorted(courseCompartor);
                OutputWriter.WriteMessageOnNewLine(list.JoinWth(Environment.NewLine));
            }

            else
                throw new InvalidCommandException(this.Input);


        }

        public IComparer<TComparable> CreateComparator<TComparable>(string sortType) where TComparable:IComparable<TComparable>
        {
            if (sortType.Equals("ascending", StringComparison.OrdinalIgnoreCase))
                return Comparer<TComparable>.Create((tc1, tc2) => tc1.CompareTo(tc2));

            else if (sortType.Equals("descending", StringComparison.OrdinalIgnoreCase))
                return Comparer<TComparable>.Create((tc1, tc2) => tc2.CompareTo(tc1));

            else throw new InvalidCommandException(this.Input);
        }
    }
}
