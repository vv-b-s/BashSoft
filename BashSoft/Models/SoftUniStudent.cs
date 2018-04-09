using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.Models
{
    public class SoftUniStudent : IStudent
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExam = 100;

        private string userName;
        private Dictionary<string, ICourse> enrolledCourses;
        private Dictionary<string, double> marksByCourseName;

        public SoftUniStudent(string userName)
        {
            UserName = userName;

            enrolledCourses = new Dictionary<string, ICourse>();
            marksByCourseName = new Dictionary<string, double>();
        }

        public string UserName
        {
            get => userName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidStringException();

                userName = value;
            }
        }

        public IReadOnlyDictionary<string, ICourse> EnrolledCourses => enrolledCourses;

        public IReadOnlyDictionary<string, double> MarksByCourseName => marksByCourseName;

        public int CompareTo(IStudent other) => this.userName.CompareTo(other.UserName);

        /// <summary>
        /// Will enroll the student in a course. Adds the student to the course and the course to the student
        /// </summary>
        /// <param name="course"></param>
        public void EnrollInCourse(ICourse course)
        {
            //If the student's enrolled courses contains the desired course name it is repeated. which means he allreadye enrolled for it
            if (EnrolledCourses.ContainsKey(course.Name))
                throw new DuplicateEntryInStructureException(userName, course.Name);

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
                throw new ArgumentException($"{userName} is not enrolled in {courseName}");

            if (scores.Length > NumberOfTasksOnExam)
                throw new ArgumentException(ExceptionMessages.InvalidNumberOfScoresException);

            marksByCourseName[courseName] = CalculateMark(scores);
        }

        public override string ToString() => this.userName;

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
