using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BashSoft
{
    public static class SessionData
    {
        private static string _curentPath;
        private static string _pathSeparator;

        public static string CurrentPath
        {
            get
            {
                if(string.IsNullOrEmpty(_curentPath))
                    _curentPath = Directory.GetCurrentDirectory();
                return _curentPath;
            }
            set { _curentPath = value; }
        }
        public static string PathSeparator
        {
            get
            {
                if (string.IsNullOrEmpty(_pathSeparator))
                    _pathSeparator = CurrentPath.Contains(@"\") ? "\\" : "/";
                return _pathSeparator;
            }
        }
    }
}
