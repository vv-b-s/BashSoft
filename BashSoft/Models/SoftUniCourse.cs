using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.IO;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Models
{
    public class SoftUniCourse : ICourse
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExam = 100;

        private string name;
        private Dictionary<string, IStudent> studentsByName;

        public SoftUniCourse(string name)
        {
            Name = name;
            studentsByName = new Dictionary<string, IStudent>();
        }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidStringException();

                name = value;
            }
        }

        public IReadOnlyDictionary<string, IStudent> StudentsByName => studentsByName;

        /// <summary>
        /// Adds a student to a course
        /// </summary>
        /// <param name="student"></param>
        public void EnrollStudent(IStudent student)
        {
            if (studentsByName.ContainsKey(student.UserName))
                throw new DuplicateEntryInStructureException(student.UserName, Name);

            else studentsByName[student.UserName] = student;
        }
    }
}
