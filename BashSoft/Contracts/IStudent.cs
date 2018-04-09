using System;
using System.Collections.Generic;

namespace BashSoft.Contracts
{
    public interface IStudent : IComparable<IStudent>
    {
        IReadOnlyDictionary<string, ICourse> EnrolledCourses { get; }
        IReadOnlyDictionary<string, double> MarksByCourseName { get; }
        string UserName { get; set; }

        void EnrollInCourse(ICourse course);
        void SetMarkOnCourse(string courseName, params int[] scores);
    }
}