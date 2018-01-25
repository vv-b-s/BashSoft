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
        public static string InvalidPathException => "The folder/file you are trying to access at the current address, does not exist.";
        public static string AccessDeniedException => "The folder/file you are trying to get access needs a higher level of rights than you currently have.";
        public static string ComparisonOfFilesWithDifferentSizesException => "Files not of equal size, certain mismatch.";
        public static string ForbiddenSymbolsContainedInNameException => "The given name contains symbols that are not allowed to be used in names of files and folders.";
        public static string InvalidUPOperationException => "Unable to go higher in partition hierarchy";
        public static string UnableToParseNumberException => "The sequence you've written is not a valid number.";
    }
}
