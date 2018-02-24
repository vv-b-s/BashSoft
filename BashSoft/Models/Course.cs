using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Models
{
    class Course
    {
        private string name;
        private Dictionary<string, Student> studentsByName;

        public static int NumberOfTasksOnExam => 5;
        public static int MaxScoreOnExam => 100;

        public Course(string name)
        {
            Name = name;
            studentsByName = new Dictionary<string, Student>();
        }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(name), ExceptionMessages.NullOrEmptyValueException);

                name = value;
            }
        }

        public IReadOnlyDictionary<string, Student> StudentsByName => studentsByName;

        /// <summary>
        /// Adds a student to a course
        /// </summary>
        /// <param name="student"></param>
        internal void EnrollStudent(Student student)
        {
            if (studentsByName.ContainsKey(student.UserName))
                throw new DuplicateWaitObjectException(string.Format(ExceptionMessages.StudentAlreadyEnrolledException, Name));

            else studentsByName[student.UserName] = student;
        }
    }
}
