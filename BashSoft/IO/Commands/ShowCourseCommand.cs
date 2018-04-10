using System;
using System.Collections.Generic;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;

namespace BashSoft.IO.Commands
{
    [Alias("show")]
    public class ShowCourseCommand : Command
    {
        public ShowCourseCommand(string input, string[] data) : base(input, data)
        {
            var dataLen = data.Length;
            if (dataLen < 2 || dataLen > 3)
                throw new InvalidCommandException(input);
        }

        [Inject]
        public IStudentsRepository Repository { get; private set; }


        public override void Execute()
        {
            var course = this.Data[1];
            if (this.Data.Length == 2)
                Repository.GetAllStudentsFromCourse(course);

            if (this.Data.Length == 3)
            {
                var username = this.Data[2];
                Repository.GetStudentScoresFromCourse(course, username);
            }
        }
    }
}
