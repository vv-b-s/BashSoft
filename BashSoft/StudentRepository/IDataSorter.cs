using System.Collections.Generic;

namespace BashSoft.Contracts
{
    public interface IDataSorter
    {
        IReadOnlyDictionary<string, double> OrderAndTake(Dictionary<string, double> studentsWithMarks, string compairson, int studentsToTake);
    }
}