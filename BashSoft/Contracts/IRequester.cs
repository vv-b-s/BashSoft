using BashSoft.StudentRepository;
using System.Collections.Generic;

namespace BashSoft.Contracts
{
    public interface IRequester
    {
        void GetAllStudentsFromCourse(string courseName);
        void GetAllStudentsFromCourse(string courseName, SortingOperation sorting, string criteria = null, int takeNumber = -1);
        void GetStudentScoresFromCourse(string courseName, string userName);

        ISimpleOrderedBag<ICourse> GetAlCoursesSorted(IComparer<ICourse> comparer);
        ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer);
    }
}