using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InexcistingCourseException : Exception
    {
        private const string InexistingCourse = "The course you are trying to get does not exist in the data base!";

        public InexcistingCourseException() : base(InexistingCourse) { }

        public InexcistingCourseException(string message) : base(message) { }
    }
}
