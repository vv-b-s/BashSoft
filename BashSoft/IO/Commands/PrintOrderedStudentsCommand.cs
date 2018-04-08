using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Contracts;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class PrintOrderedStudentsCommand : ModifiablePrintCommand
    {
        public PrintOrderedStudentsCommand(string input, string[] data, IContentComparer judge, IStudentsRepository repository, IDirectoryManager iOManager) : base(input, data, judge, repository, iOManager)
        {
        }

        public override SortingOperation SortingOperation => SortingOperation.Order;
    }
}
