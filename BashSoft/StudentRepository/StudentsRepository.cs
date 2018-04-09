using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BashSoft.Contracts;
using BashSoft.DataStructures;
using BashSoft.Exceptions;
using BashSoft.IO;
using BashSoft.Models;
using BashSoft.StaticData;
using BashSoft.StudentRepository.Filtering;

namespace BashSoft.StudentRepository
{
    public class StudentsRepository : IStudentsRepository
    {
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;

        private bool isDataInitialized = false;
        private IDataFilter filter;
        private IDataSorter sorter;
        private Regex matcher;

        public StudentsRepository(IDataSorter sorter, IDataFilter filter)
        {
            this.filter = filter;
            this.sorter = sorter;

            courses = new Dictionary<string, ICourse>();
            students = new Dictionary<string, IStudent>();
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
                throw new DataInitializationException(true);
        }

        /// <summary>
        /// Clears the loaded data and sets it to not initialized
        /// </summary>
        public void UnloadData()
        {
            if (!isDataInitialized)
                throw new DataInitializationException(false);

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

                            if (scores.Any(s => s > SoftUniStudent.MaxScoreOnExam || s < 0))
                                OutputWriter.DisplayException(ExceptionMessages.InvalidScoreException);

                            if (scores.Length > SoftUniCourse.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScoresException);
                                continue;
                            }

                            if (!HasStudent(studentName))
                                students[studentName] = new SoftUniStudent(studentName);

                            if (!HasCourse(courseName))
                                courses[courseName] = new SoftUniCourse(courseName);

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
                else throw new InvalidPathException();
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
                throw new InexcistingCourseException();

            //Otherwise if nothing exists
            else
                throw new DataInitializationException(false);
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
            throw new InexistingStudentException();
        }

        /// <summary>
        /// Will print the student of the course and his/hers scores
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="userName"></param>
        public void GetStudentScoresFromCourse(string courseName, string userName)
        {
            if (IsQueryForStudentPossible(courseName, userName))
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
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var student in courses[courseName].StudentsByName)
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
        /// <param name="sorting">Choose either Filtering or Ordering</param>
        /// <param name="criteria">Filtering - by performance, Sorting - Ascending, Descending</param>
        /// <param name="takeNumber">Amount of items to take</param>
        public void GetAllStudentsFromCourse(string courseName, SortingOperation sorting, string criteria = null, int takeNumber = -1)
        {

            if (sorting == SortingOperation.None)
                GetAllStudentsFromCourse(courseName);
            else if (IsQueryForCoursePossible(courseName))
            {
                //Extract the student names and their marks
                var studentsWithMarks = courses[courseName].StudentsByName.ToDictionary(k => k.Key, v => v.Value.MarksByCourseName[courseName]);

                if (sorting == SortingOperation.Filter)
                    studentsWithMarks = filter.FilterAndTake(studentsWithMarks, criteria, takeNumber) as Dictionary<string, double>;

                else if (sorting == SortingOperation.Order)
                    studentsWithMarks = sorter.OrderAndTake(studentsWithMarks, criteria, takeNumber) as Dictionary<string, double>;

                if (studentsWithMarks is null)
                    return;

                foreach (var student in studentsWithMarks)
                    OutputWriter.PrintStudent(student);
            }
        }

        public bool HasCourse(string courseName) => courses.ContainsKey(courseName);
        public bool HasStudent(string studentName) => courses.ContainsKey(studentName);

        public ISimpleOrderedBag<ICourse> GetAlCoursesSorted(IComparer<ICourse> comparer)
        {
            var sortedCourses = new SimpleSortedList<ICourse>(comparer);
            sortedCourses.AddAll(this.courses.Values);

            return sortedCourses;
        }

        public ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer)
        {
            var sortedStudents = new SimpleSortedList<IStudent>(comparer);
            sortedStudents.AddAll(this.students.Values);

            return sortedStudents;
        }
    }
}
