using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    [Alias("filter")]
    public class PrintFilteredStudentsCommand : ModifiablePrintCommand
    {
        public PrintFilteredStudentsCommand(string input, string[] data) : base(input, data) { }

        public override SortingOperation SortingOperation => SortingOperation.Filter;
    }
}
