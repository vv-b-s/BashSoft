using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BashSoft.IO;
using static BashSoft.IO.OutputWriter;

namespace BashSoft.Repositories
{
    public static class StudentsRepository
    {
        public static bool isDataInitialized = false;
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;

        /// <summary>
        /// Initialize students data. From a file in the current location
        /// </summary>
        public static void InitializeData(string fileName)
        {
            if (!isDataInitialized)
            {
                WriteMessageOnNewLine("Reading data...");
                studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
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
            var path = SessionData.CurrentPath + SessionData.PathSeparator + fileName;
            if (File.Exists(path))
            {
                var lines = new Queue<string>(File.ReadAllLines(path));

                while (lines.Count > 0)
                {
                    var tokens = lines.Dequeue().Split(" ");

                    var course = tokens[0];
                    var student = tokens[1];
                    int.TryParse(tokens[2], out int mark);

                    //If there is no such course, create one
                    if (!studentsByCourse.ContainsKey(course))
                        studentsByCourse[course] = new Dictionary<string, List<int>>();

                    //If there is no such student, create one
                    if (!studentsByCourse[course].ContainsKey(student))
                        studentsByCourse[course][student] = new List<int>();

                    //Add the mark to the student
                    studentsByCourse[course][student].Add(mark);
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
        public static void PrintStudent(KeyValuePair<string, List<int>> student) => OutputWriter.PrintStudent(student.Key, student.Value);

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
