using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BashSoft.IO;
using BashSoft.StaticData;

namespace BashSoft.StudentRepository.Filtering
{
    /**
     * Here are all the public methods for filters
     * Partial clases are used to split a class for convenience
     */
    public static partial class Filters
    {
        /// <summary>
        /// Filters students from a certain course by the given filter and takes as many as needed
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="wantedFilter"></param>
        /// <param name="studentsToTake"></param>
        public static void FilterAndTake(string courseName, string wantedFilter, int studentsToTake)
        {
            if (StudentsRepository.studentsByCourse.ContainsKey(courseName))
            {
                //If the studentsToTake is -1, it will take all the students from the list
                if (studentsToTake == -1)
                    studentsToTake = StudentsRepository.studentsByCourse[courseName].Count;

                //NOTE: Filter methods are kept for better readability
                if (wantedFilter == "exelent")
                    FilterAndTake(StudentsRepository.studentsByCourse[courseName], ExelentFilter, studentsToTake);
                else if (wantedFilter == "average")
                    FilterAndTake(StudentsRepository.studentsByCourse[courseName], AverageFilter, studentsToTake);
                else if (wantedFilter == "poor")
                    FilterAndTake(StudentsRepository.studentsByCourse[courseName], PoorFilter, studentsToTake);
                else OutputWriter.DisplayException(ExceptionMessages.InvalidStudentFilterException);

            }
            else OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBaseException);
        }
    }
}
