using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidDataException : Exception
    {
        private const string InvalidData = "Invalid data: {0}";

        public InvalidDataException(string dataArgument) : base(string.Format(InvalidData, dataArgument)) { }
    }
}
