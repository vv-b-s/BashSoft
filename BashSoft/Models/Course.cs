using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Models
{
    class Course
    {
        public string Name { get; set; }
        public Dictionary<string, Student> StudentsByName { get; private set; }

        public static int NumberOfTasksOnExam => 5;
        public static int MaxScoreOnExam => 100;

        public Course(string name)
        {
            Name = name;
            StudentsByName = new Dictionary<string, Student>();
        }

        /// <summary>
        /// Adds a student to a course
        /// </summary>
        /// <param name="student"></param>
        internal void EnrollStudent(Student student)
        {
            if (StudentsByName.ContainsKey(student.UserName))
                OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledException, this.Name));

            else StudentsByName[student.UserName] = student;
        }
    }
}
