using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    public static class ExceptionMessages
    {
        /** 
         * Add only messages in the format: public static string ExampleExceptionMessage => "Example message!";
         * Read-only properties are immutable
         */

        public static string DataAlreadyInitialisedException => "Data is already initialized!";
        public static string DataNotInitializedException => "The data structure must be initialised first in order to make any operations with it.";
        public static string InexistingCourseInDataBaseException => "The course you are trying to get does not exist in the data base!";
        public static string InexistingStudentInDataBaseException => "The user name for the student you are trying to get does not exist!";
    }
}
