using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BashSoft.Contracts;
using BashSoft.IO;
using BashSoft.StaticData;

namespace BashSoft.StudentRepository.Filtering
{
    /**
     * Here are all the public methods for filters
     * Partial clases are used to split a class for convenience
     */
    public partial class Filter : IDataFilter
    {
        /// <summary>
        /// Filters students from a certain course by the given filter and takes as many as needed
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="wantedFilter"></param>
        /// <param name="studentsToTake"></param>
        public IReadOnlyDictionary<string, double> FilterAndTake(Dictionary<string, double> studentsWithMarks, string wantedFilter, int studentsToTake)
        {
            //If the studentsToTake is -1, it will take all the students from the list
            if (studentsToTake == -1)
                studentsToTake = studentsWithMarks.Count;

            //NOTE: Filter methods are kept for better readability
            if (wantedFilter == "exelent")
                return FilterAndTake(studentsWithMarks, ExelentFilter, studentsToTake);
            else if (wantedFilter == "average")
                return FilterAndTake(studentsWithMarks, AverageFilter, studentsToTake);
            else if (wantedFilter == "poor")
                return FilterAndTake(studentsWithMarks, PoorFilter, studentsToTake);
            else OutputWriter.DisplayException(ExceptionMessages.InvalidStudentFilterException);
            return null;
        }
    }
}
