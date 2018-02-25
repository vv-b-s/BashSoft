using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidCommandException : Exception
    {
        private const string InvalidCommand = "Invalid command: {0}";

        /// <summary>
        /// Place the command into a default massage. Cannot use custom message with this exception.
        /// </summary>
        /// <param name="command"></param>
        public InvalidCommandException(string command) : base(string.Format(InvalidCommand, command)) { }
    }
}
