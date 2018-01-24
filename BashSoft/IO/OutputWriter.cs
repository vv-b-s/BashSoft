using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO
{
    public static class OutputWriter
    {
        /// <summary>
        /// Invokes System.Console.Write() and displays a message on the Console
        /// </summary>
        /// <param name="message"></param>
        public static void WriteMessage(string message) => Console.Write(message);

        /// <summary>
        /// Invokes System.Console.WriteLine() and displays a message on the Console, ednding with a new line
        /// </summary>
        /// <param name="message"></param>
        public static void WriteMessageOnNewLine(string message) => Console.WriteLine(message);

        /// <summary>
        /// Invokes System.Console.Write() and prints an empty line
        /// </summary>
        public static void WriteEmptyLine() => Console.WriteLine();

        /// <summary>
        /// Invokes System.Console.Write() and displays an error on the Console. It is colored in red.
        /// </summary>
        /// <param name="message"></param>
        public static void DisplayException(string message)
        {
            //Changing the color property of the console text only to show the message in different color and than turning it back to the default
            var defaultConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultConsoleColor;
        }

        /// <summary>
        /// Prints student data by given name and marks
        /// </summary>
        /// <param name="studentName"></param>
        /// <param name="studentMarks"></param>
        public static void PrintStudent(string studentName, List<int> studentMarks) =>
            WriteMessageOnNewLine($"{studentName} - {string.Join(", ", studentMarks)}");
    }
}
