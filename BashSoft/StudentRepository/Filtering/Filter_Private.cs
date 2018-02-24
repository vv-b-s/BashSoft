using BashSoft.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.StudentRepository.Filtering
{
    /**
     *  Here are all the private methods for filters 
     *  Partial clases are used to split a class for convenience
     */
    public partial class Filter
    {
        /// <summary>
        /// Does the actual filtration given from the pubic method
        /// </summary>
        /// <param name="studentsWithMarks"></param>
        /// <param name="givenFilter"></param>
        /// <param name="studentsToTake"></param>
        /// <returns></returns>
        private Dictionary<string,double> FilterAndTake(Dictionary<string, double> studentsWithMarks, Predicate<double> givenFilter, int studentsToTake)
        {
            //Get the students which mark matches the criteria of the given filter and take the desired number of students
            studentsWithMarks = studentsWithMarks
                .Where(mark => givenFilter(mark.Value))
                .Take(studentsToTake)
                .ToDictionary(student=>student.Key, scores=>scores.Value);

            return studentsWithMarks;
        }

        /// <summary>
        /// True if mark is equal or above 5
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private bool ExelentFilter(double mark) => mark >= 5.0;

        /// <summary>
        /// True if mark is between 3.5 and 4.9
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private bool AverageFilter(double mark) => mark < 5.0 && mark >= 3.5;

        /// <summary>
        /// True if mark is under 3.5
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private bool PoorFilter(double mark) => mark < 3.5;
    }
}
