using System.Collections.Generic;

namespace BashSoft.Contracts
{
    public interface IDataFilter
    {
        IReadOnlyDictionary<string, double> FilterAndTake(Dictionary<string, double> studentsWithMarks, string wantedFilter, int studentsToTake);
    }
}