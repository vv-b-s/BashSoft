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
        private string userName;
        private Dictionary<string, Course> enrolledCourses;
        private Dictionary<string, double> marksByCourseName;

        public Student(string userName)
        {
            UserName = userName;

            enrolledCourses = new Dictionary<string, Course>();
            marksByCourseName = new Dictionary<string, double>();
        }

        public string UserName
        {
            get => userName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(userName), ExceptionMessages.NullOrEmptyValueException);

                userName = value;
            }
        }

        public IReadOnlyDictionary<string, Course> EnrolledCourses => enrolledCourses;

        public IReadOnlyDictionary<string, double> MarksByCourseName => marksByCourseName;

        /// <summary>
        /// Will enroll the student in a course. Adds the student to the course and the course to the student
        /// </summary>
        /// <param name="course"></param>
        public void EnrollInCourse(Course course)
        {
            if (EnrolledCourses.ContainsKey(course.Name))
                throw new DuplicateWaitObjectException(string.Format(ExceptionMessages.StudentAlreadyEnrolledException, course.Name));
            else
            {
                enrolledCourses[course.Name] = course;
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
                throw new DuplicateWaitObjectException(string.Format(ExceptionMessages.StudentAlreadyEnrolledException, courseName));

            if (scores.Length > Course.NumberOfTasksOnExam)
                throw new ArgumentException(ExceptionMessages.InvalidNumberOfScoresException);

            marksByCourseName[courseName] = CalculateMark(scores);
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
