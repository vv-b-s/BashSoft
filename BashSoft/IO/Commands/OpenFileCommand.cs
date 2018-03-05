using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StaticData;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    public class OpenFileCommand : Command
    {
        public OpenFileCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager iOManager) : base(input, data, judge, repository, iOManager)
        {
            if (data.Length != 2)
                throw new InvalidCommandException(input);
        }

        public override void Execute()
        {
            var fileName = this.Data[1];

            //Use current path if it is not absolute
            if (!fileName.Contains(SessionData.PathSeparator))
                fileName = SessionData.CurrentPath + SessionData.PathSeparator + fileName;

            //https://stackoverflow.com/questions/4055266/open-a-file-with-notepad-in-c-sharp
            var process = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    //Tells OS to use the default program
                    UseShellExecute = true,
                    FileName = fileName
                }
            };

            process.Start();
        }
    }
}
