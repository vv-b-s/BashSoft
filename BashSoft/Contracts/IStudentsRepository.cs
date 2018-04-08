using BashSoft.StudentRepository;

namespace BashSoft.Contracts
{
    public enum SortingOperation { None, Filter, Order }
    public interface IStudentsRepository : IRequester
    {
        bool HasCourse(string courseName);
        bool HasStudent(string studentName);
        void LoadData(string fileName);
        void UnloadData();
    }
}