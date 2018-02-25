using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidStringException : Exception
    {
        private const string InvalidString = "The value of the variable CANNOT be null or empty!";

        public InvalidStringException() : base(InvalidString) { }

        public InvalidStringException(string message) : base(message) { }
    }
}
