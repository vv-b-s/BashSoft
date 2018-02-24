using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using BashSoft.IO;
using BashSoft.Models;
using BashSoft.StaticData;
using BashSoft.StudentRepository.Filtering;

namespace BashSoft.StudentRepository
{
    public enum SortingOperation { None, Filter, Order}
    public class StudentsRepository
    {
        private Dictionary<string, Course> courses;
        private Dictionary<string, Student> students;

        private bool isDataInitialized = false;
        private Filter filter;
        private Sorter sorter;
        private Regex matcher;

        public StudentsRepository(Sorter sorter, Filter filter)
        {
            this.filter = filter;
            this.sorter = sorter;

            courses = new Dictionary<string, Course>();
            students = new Dictionary<string, Student>();
        }

        /// <summary>
        /// Initialize students data. From a file in the current location
        /// </summary>
        public void LoadData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data...");
                matcher = new Regex(@"(?<courseName>[A-Z][a-zA-Z#\++]*_[A-Z][a-z]{2}_\d{4})\s+(?<userName>[A-Za-z]+\d{2}_\d{2,4})\s(?<scores>[\s0-9]+)");
                ReadData(fileName);
            }
            else
                throw new ArgumentException(ExceptionMessages.DataAlreadyInitialisedException);
        }

        /// <summary>
        /// Clears the loaded data and sets it to not initialized
        /// </summary>
        public void UnloadData()
        {
            if (!isDataInitialized)
                throw new ArgumentException(ExceptionMessages.DataNotInitializedException);

            //Clearing the dictionary is better than new initialization.
            courses.Clear();
            students.Clear();

            isDataInitialized = false;

            OutputWriter.WriteMessageOnNewLine("Database dropped!");
        }

        /// <summary>
        /// Reads the given data of the courses and students and scores and initializes it in a dictionary
        /// </summary>
        private void ReadData(string fileName)
        {
            //Keep track on the line number of the file to show which line is broken in the stacktrace
            var lineNumber = 0;

            try
            {
                var path = fileName;

                //If the path doesn't contain even one path separator it means it is relative path and full path needs to be pointed out
                if (!path.Contains(SessionData.PathSeparator))
                    path = SessionData.CurrentPath + SessionData.PathSeparator + fileName;

                if (File.Exists(path))
                {
                    var lines = new Queue<string>(File.ReadAllLines(path));

                    while (lines.Count > 0)
                    {
                        lineNumber++;
                        var matches = matcher.Match(lines.Dequeue());
                        if (matches.Value.Length != 0)
                        {
                            var courseName = matches.Groups["courseName"].Value;
                            var studentName = matches.Groups["userName"].Value;
                            var scores = matches.Groups["scores"].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                            if (scores.Any(s => s > 100 || s < 0))
                                OutputWriter.DisplayException(ExceptionMessages.InvalidScoreException);

                            if (scores.Length > Course.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScoresException);
                                continue;
                            }

                            if (!HasStudent(studentName))
                                students[studentName] = new Student(studentName);

                            if (!HasCourse(courseName))
                                courses[courseName] = new Course(courseName);

                            var student = students[studentName];
                            var course = courses[courseName];

                            //The method will also enroll the student in the course class :)
                            student.EnrollInCourse(course);

                            student.SetMarkOnCourse(courseName, scores);
                        }
                    }

                    isDataInitialized = true;
                    OutputWriter.WriteMessageOnNewLine("Data read!");
                }
                else throw new ArgumentException(ExceptionMessages.InvalidPathException);
            }
            catch (FormatException fex)
            {
                OutputWriter.DisplayException(fex, lineNumber);
            }
        }

        /// <summary>
        /// Checks whether if the course exists in the Data Base.
        /// If not or the data base is not initialized will display an exception message
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        private bool IsQueryForCoursePossible(string courseName)
        {
            //If the data is initialized and the course exists
            if (isDataInitialized && HasCourse(courseName))
                return true;

            //Else if data is initialized but the course does not exist
            else if (isDataInitialized && !HasCourse(courseName))
                throw new ArgumentException(ExceptionMessages.InexistingCourseInDataBaseException);

            //Otherwise if nothing exists
            else
                throw new ArgumentException(ExceptionMessages.DataNotInitializedException);
        }

        /// <summary>
        /// Checks whether if the student's name exists in the database
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="studentUserName"></param>
        /// <returns></returns>
        private bool IsQueryForStudentPossible(string courseName, string studentUserName)
        {
            //If the course name is valid and the student exists in that course
            if (IsQueryForCoursePossible(courseName) && courses[courseName].StudentsByName.ContainsKey(studentUserName))
                return true;

            //Otherewise will dislpay exception message
            throw new ArgumentException(ExceptionMessages.InexistingStudentInDataBaseException);
        }

        /// <summary>
        /// Will print the student of the course and his/hers scores
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="userName"></param>
        public void GetStudentScoresFromCourse(string courseName, string userName)
        {
            if(IsQueryForStudentPossible(courseName, userName))
            {
                var student = new KeyValuePair<string, double>(userName, students[userName].MarksByCourseName[courseName]);
                OutputWriter.PrintStudent(student);
            }
        }

        /// <summary>
        /// Gets all the students from the course and lists their names and grades
        /// </summary>
        /// <param name="courseName"></param>
        public void GetAllStudentsFromCourse(string courseName)
        {
            if(IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach(var student in courses[courseName].StudentsByName)
                {
                    var studentMark = new KeyValuePair<string, double>(student.Value.UserName, student.Value.MarksByCourseName[courseName]);
                    OutputWriter.PrintStudent(studentMark);
                }
            }
        }

        /// <summary>
        /// Gets students but also does filtering query on them
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="sorting"></param>
        /// <param name="criteria"></param>
        /// <param name="takeNumber"></param>
        public void GetAllStudentsFromCourse(string courseName, SortingOperation sorting, string criteria = null, int takeNumber = -1)
        {

            if (sorting == SortingOperation.None)
                GetAllStudentsFromCourse(courseName);
            else if (IsQueryForCoursePossible(courseName))
            {
                //Extract the student names and their marks
                var studentsWithMarks = courses[courseName].StudentsByName.ToDictionary(k => k.Key, v => v.Value.MarksByCourseName[courseName]);

                if (sorting == SortingOperation.Filter)
                    studentsWithMarks = filter.FilterAndTake(studentsWithMarks, criteria, takeNumber);

                else if (sorting == SortingOperation.Order)
                    studentsWithMarks = sorter.OrderAndTake(studentsWithMarks, criteria, takeNumber);

                if (studentsWithMarks is null)
                    return;

                foreach (var student in studentsWithMarks)
                    OutputWriter.PrintStudent(student);
            }                 
        }

        public bool HasCourse(string courseName) => courses.ContainsKey(courseName);
        public bool HasStudent(string studentName) => courses.ContainsKey(studentName);
    }
}
