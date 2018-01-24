using System;

using BashSoft.IO;
using BashSoft.Repositories;

namespace BashSoft
{
    class Launcher
    {
        static void Main(string[] args)
        {
            StudentsRepository.InitializeData();
            StudentsRepository.GetAllStudentsFromCourse("Unity");
        }
    }
}
