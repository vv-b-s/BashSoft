﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using BashSoft.IO;
using static BashSoft.IO.OutputWriter;

namespace BashSoft.Repositories
{
    public static class StudentsRepository
    {
        public static bool isDataInitialized = false;
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;
        public static Regex matcher;

        /// <summary>
        /// Initialize students data. From a file in the current location
        /// </summary>
        public static void InitializeData(string fileName)
        {
            if (!isDataInitialized)
            {
                WriteMessageOnNewLine("Reading data...");
                studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                matcher = new Regex(@"^(?<courseName>[A-Z][A-Za-z\+#]+_[JFMASOND][a-z]+_\d{4})\s(?<userName>[A-Z][a-z]{0,3}\d{2}_\d{2,4})\s(?<score>\d+)$");
                ReadData(fileName);
            }
            else
                DisplayException(ExceptionMessages.DataAlreadyInitialisedException);
        }

        /// <summary>
        /// Reads the given data of the courses and students and marks and initializes it in a dictionary
        /// </summary>
        private static void ReadData(string fileName)
        {
            var path = fileName;

            //If the path doesn't contain even one path separator it means it is relative path and full path needs to be pointed out
            if(!path.Contains(SessionData.PathSeparator))
                path = SessionData.CurrentPath + SessionData.PathSeparator + fileName;

            if (File.Exists(path))
            {
                var lines = new Queue<string>(File.ReadAllLines(path));

                while (lines.Count > 0)
                {
                    var matches = matcher.Match(lines.Dequeue());
                    if (matches.Value.Length!=0)
                    {
                        var course = matches.Groups["courseName"].Value;
                        var student = matches.Groups["userName"].Value;
                        int.TryParse(matches.Groups["score"].Value, out int mark);

                        //If the mark is not between 0 and 100 the data will not be added
                        if (mark < 0 || mark > 100)
                            continue;

                        //If there is no such course, create one
                        if (!studentsByCourse.ContainsKey(course))
                            studentsByCourse[course] = new Dictionary<string, List<int>>();

                        //If there is no such student, create one
                        if (!studentsByCourse[course].ContainsKey(student))
                            studentsByCourse[course][student] = new List<int>();

                        //Add the mark to the student
                        studentsByCourse[course][student].Add(mark); 
                    }
                }

                isDataInitialized = true;
                WriteMessageOnNewLine("Data read!");
            }
            else DisplayException(ExceptionMessages.InvalidPathException);
        }

        /// <summary>
        /// Checks whether if the course exists in the Data Base.
        /// If not or the data base is not initialized will display an exception message
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        private static bool IsQueryForCoursePossible(string courseName)
        {
            //If the data is initialized and the course exists
            if (isDataInitialized && studentsByCourse.ContainsKey(courseName))
                return true;

            //Else if data is initialized but the course does not exist
            else if (isDataInitialized && !studentsByCourse.ContainsKey(courseName))
                DisplayException(ExceptionMessages.InexistingCourseInDataBaseException);

            //Otherwise if nothing exists
            else
                DisplayException(ExceptionMessages.DataNotInitializedException);

            return false;
        }

        /// <summary>
        /// Checks whether if the student's name exists in the database
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="studentUserName"></param>
        /// <returns></returns>
        private static bool IsQueryForStudentPossible(string courseName, string studentUserName)
        {
            //If the course name is valid and the student exists in that course
            if (IsQueryForCoursePossible(courseName) && studentsByCourse[courseName].ContainsKey(studentUserName))
                return true;

            //Otherewise will dislpay exception message
            DisplayException(ExceptionMessages.InexistingStudentInDataBaseException);

            return false;
        }

        /// <summary>
        /// Sends student information to the OutputWriter
        /// </summary>
        /// <param name="student"></param>
        private static void PrintStudent(KeyValuePair<string, List<int>> student) => OutputWriter.PrintStudent(student.Key, student.Value);

        /// <summary>
        /// Will print the student of the course and his/hers marks
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="userName"></param>
        public static void GetStudentScoresFromCourse(string courseName, string userName)
        {
            if(IsQueryForStudentPossible(courseName, userName))
            {
                var studentMarks = studentsByCourse[courseName][userName];
                OutputWriter.PrintStudent(userName, studentMarks);
            }
        }

        /// <summary>
        /// Gets all the students from the course and lists their names and grades
        /// </summary>
        /// <param name="courseName"></param>
        public static void GetAllStudentsFromCourse(string courseName)
        {
            if(IsQueryForCoursePossible(courseName))
            {
                WriteMessageOnNewLine($"{courseName}:");
                foreach (var student in studentsByCourse[courseName])
                    PrintStudent(student);
            }
        }
    }
}
