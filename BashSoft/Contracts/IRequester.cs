using BashSoft.StudentRepository;

namespace BashSoft.Contracts
{
    public interface IRequester
    {
        void GetAllStudentsFromCourse(string courseName);
        void GetAllStudentsFromCourse(string courseName, SortingOperation sorting, string criteria = null, int takeNumber = -1);
        void GetStudentScoresFromCourse(string courseName, string userName);
    }
}