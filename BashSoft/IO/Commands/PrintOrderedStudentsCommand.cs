using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    [Alias("order")]
    public class PrintOrderedStudentsCommand : ModifiablePrintCommand
    {
        public PrintOrderedStudentsCommand(string input, string[] data) : base(input, data) { }

        public override SortingOperation SortingOperation => SortingOperation.Order;
    }
}
