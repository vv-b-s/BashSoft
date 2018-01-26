using System;
using System.Collections.Generic;
using System.Text;

using BashSoft.IO;

namespace BashSoft.StaticData
{
    public static class Help
    {
        public static void DisplayHelpMessage()
        {
            var message = new StringBuilder();
            message.Append($"{new string('_', 127)}\n");
            message.AppendFormat("|{0, -125}|\n", "make directory - mkdir: path ");
            message.AppendFormat("|{0, -125}|\n", "traverse directory - ls: depth ");
            message.AppendFormat("|{0, -125}|\n", "comparing files - cmp: path1 path2");
            message.AppendFormat("|{0, -125}|\n", "change directory - changeDirREl:relative path");
            message.AppendFormat("|{0, -125}|\n", "change directory - changeDir:absolute path");
            message.AppendFormat("|{0, -125}|\n", "read students data base - readDb: path");
            message.AppendFormat("|{0, -125}|\n", "show courseName (username) - user name may be omitted");
            message.AppendFormat("|{0, -125}|\n", "filter {courseName} excelent/average/poor  take 2/5/all students - filterExcelent (the output is written on the console)");
            message.AppendFormat("|{0, -125}|\n", "order increasing students - order {courseName} ascending/descending take 20/10/all (the output is written on the console)");
            message.AppendFormat("|{0, -125}|\n", "download file - download: path of file (saved in current directory)");
            message.AppendFormat("|{0, -125}|\n", "download file asinchronously - downloadAsynch: path of file (save in the current directory)");
            message.AppendFormat("|{0, -125}|\n", "get help – help");
            message.Append($"{new string('_', 127)}");

            OutputWriter.WriteMessageOnNewLine(message.ToString());
        }
    }
}
