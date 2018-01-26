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
    public static partial class Filters
    {
        /// <summary>
        /// Does the actual filtration given from the pubic method
        /// </summary>
        /// <param name="studentsAndScores"></param>
        /// <param name="givenFilter"></param>
        /// <param name="studentsToTake"></param>
        private static void FilterAndTake(Dictionary<string, List<int>> studentsAndScores, Predicate<double> givenFilter, int studentsToTake)
        {
            //Filter out the needed students
            studentsAndScores = studentsAndScores
                .Where(Scores => givenFilter(Average(Scores.Value)))
                .Take(studentsToTake)
                .ToDictionary(student=>student.Key, scores=>scores.Value);

            //Print the data
            StudentsRepository.PrintAllStudentsFromCourse(studentsAndScores);
        }

        /// <summary>
        /// True if mark is equal or above 5
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private static bool ExelentFilter(double mark) => mark >= 5.0;

        /// <summary>
        /// True if mark is between 3.5 and 4.9
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private static bool AverageFilter(double mark) => mark < 5.0 && mark >= 3.5;

        /// <summary>
        /// True if mark is under 3.5
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private static bool PoorFilter(double mark) => mark < 3.5;

        /// <summary>
        /// Gets the average mark by calculating the scores
        /// </summary>
        /// <param name="scoresOnTasks"></param>
        /// <returns></returns>
        private static double Average(List<int> scoresOnTasks)
        {
            var totalScore = scoresOnTasks.Sum();

            var percentageOfAll = totalScore / (scoresOnTasks.Count * 100.0);
            var mark = percentageOfAll * 4 + 2;

            return mark;
        }
    }
}
