using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InexistingStudentException:Exception
    {
        private const string InexistingStudent = "The user name for the student you are trying to get does not exist!";

        public InexistingStudentException() : base(InexistingStudent) { }

        public InexistingStudentException(string message) : base(message) { }
    }
}
