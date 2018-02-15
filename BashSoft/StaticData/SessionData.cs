﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BashSoft.StudentRepository;

namespace BashSoft.StaticData
{
    public static class SessionData
    {
        private static string _curentPath;
        private static string _pathSeparator;

        public static string CurrentPath
        {
            get
            {
                if (string.IsNullOrEmpty(_curentPath))
                    _curentPath = Directory.GetCurrentDirectory();
                return _curentPath;
            }
            set { _curentPath = value; }
        }

        //In some operating systems like Linux the path slash is '/' but in Windows is '\' so this needs to be caught on time
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