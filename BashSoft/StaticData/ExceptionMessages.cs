using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.StaticData
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
        public static string InvalidStudentFilterException => "The given filter is not one of the following: excellent/average/poor";
        public static string InvalidComparisonQueryException => "The comparison query you want, does not exist in the context of the current program!";
        public static string InvalidCommandException => "Invalid command: {0}";
        public static string InvalidDataException => "Invalid data: {0}";
        public static string StudentAlreadyEnrolledException => "The student exists in {0}";
        public static string InvalidNumberOfScoresException => "The number of scores for the given course is greater than the possible.";
        public static string InvalidScoreException => "Scores must be between 0 and 100!";
        public static string NullOrEmptyValueException => "The value of the variable CANNOT be null or empty!";
    }
}
