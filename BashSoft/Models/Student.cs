using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.Models
{
    class Student
    {
        public string UserName { get; set; }
        public Dictionary<string, Course> EnrolledCourses { get; private set; }
        public Dictionary<string, double> MarksByCourseName { get; private set; }

        public Student(string userName)
        {
            UserName = userName;

            EnrolledCourses = new Dictionary<string, Course>();
            MarksByCourseName = new Dictionary<string, double>();
        }

        /// <summary>
        /// Will enroll the student in a course. Adds the student to the course and the course to the student
        /// </summary>
        /// <param name="course"></param>
        public void EnrollInCourse(Course course)
        {
            if (EnrolledCourses.ContainsKey(course.Name))
                OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledException, course.Name));
            else
            {
                EnrolledCourses[course.Name] = course;
                course.EnrollStudent(this);
            }
        }

        /// <summary>
        /// Sets a mark for the student on a certain course by calculating the average mark based on scores
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="scores"></param>
        public void SetMarkOnCourse(string courseName, params int[] scores)
        {
            if (!EnrolledCourses.ContainsKey(courseName))
            {
                OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledException, courseName));
                return;
            }

            if (scores.Length > Course.NumberOfTasksOnExam)
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScoresException);
                return;
            }

            MarksByCourseName[courseName] = CalculateMark(scores);
        }

        /// <summary>
        /// Gets the average mark by calculating the scores
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        private double CalculateMark(int[] scores)
        {
            var totalScore = scores.Sum();

            var percentageOfAll = totalScore / (scores.Length * 100.0);
            var mark = percentageOfAll * 4 + 2;

            return mark;
        }
    }
}
