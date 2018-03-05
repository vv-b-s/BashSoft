using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class PrintFilteredStudentsCommand : ModifiablePrintCommand
    {
        public PrintFilteredStudentsCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager iOManager) : base(input, data, judge, repository, iOManager)
        {
        }

        public override SortingOperation SortingOperation => SortingOperation.Filter;
    }
}
