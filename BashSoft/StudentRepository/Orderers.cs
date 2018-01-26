using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.StudentRepository
{
    public static class Orderer
    {
        /// <summary>
        /// Orders the given course by either ascending or descending order
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="compairson"></param>
        /// <param name="studentsToTake"></param>
        public static void OrderAndTake(string courseName, string compairson, int studentsToTake)
        {
            if (StudentsRepository.studentsByCourse.ContainsKey(courseName))
            {
                //If the studentsToTake is -1, it will take all the students from the list
                if (studentsToTake == -1)
                    studentsToTake = StudentsRepository.studentsByCourse[courseName].Count;

                if (compairson == "ascending")
                {
                    var orderedStudents = StudentsRepository.studentsByCourse[courseName]
                        .OrderBy(studentScores => studentScores.Value.Sum())
                        .Take(studentsToTake)
                        .ToDictionary(student => student.Key, scores => scores.Value);

                    StudentsRepository.PrintAllStudentsFromCourse(orderedStudents);
                }
                else if (compairson == "descending")
                {
                    var orderedStudents = StudentsRepository.studentsByCourse[courseName]
                        .OrderByDescending(studentScores => studentScores.Value.Sum())
                        .Take(studentsToTake)
                        .ToDictionary(student => student.Key, scores => scores.Value);

                    StudentsRepository.PrintAllStudentsFromCourse(orderedStudents);
                }
                else
                    OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQueryException);
            }
            else
                OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBaseException);
        }
    }
}
