using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.StudentRepository
{
    public class Sorter
    {
        /// <summary>
        /// Orders the given course by either ascending or descending order
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="compairson"></param>
        /// <param name="studentsToTake"></param>
        public Dictionary<string, double> OrderAndTake(Dictionary<string, double> studentsWithMarks, string compairson, int studentsToTake)
        {
            //If the studentsToTake is -1, it will take all the students from the list
            if (studentsToTake == -1)
                studentsToTake = studentsWithMarks.Count;

            if (compairson == "ascending")
            {
                //Sorts the students by their scores and get take as many as desired
                var orderedStudents = studentsWithMarks
                    .OrderBy(studentScores => studentScores.Value)
                    .Take(studentsToTake)
                    .ToDictionary(student => student.Key, scores => scores.Value);

                return orderedStudents;
            }
            else if (compairson == "descending")
            {
                //Sorts the students by their scores and get take as many as desired
                var orderedStudents = studentsWithMarks
                    .OrderByDescending(studentScores => studentScores.Value)
                    .Take(studentsToTake)
                    .ToDictionary(student => student.Key, scores => scores.Value);

                return orderedStudents;
            }
            else
                OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQueryException);

            return null;
        }
    }
}
