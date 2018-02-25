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
        public static string AccessDeniedException => "The folder/file you are trying to get access needs a higher level of rights than you currently have.";
        public static string ComparisonOfFilesWithDifferentSizesException => "Files not of equal size, certain mismatch.";
        public static string InvalidUPOperationException => "Unable to go higher in partition hierarchy";
        public static string UnableToParseNumberException => "The sequence you've written is not a valid number.";
        public static string InvalidStudentFilterException => "The given filter is not one of the following: excellent/average/poor";
        public static string InvalidComparisonQueryException => "The comparison query you want, does not exist in the context of the current program!";
        public static string InvalidNumberOfScoresException => "The number of scores for the given course is greater than the possible.";
        public static string InvalidScoreException => "Scores must be between 0 and 100!";
    }
}
