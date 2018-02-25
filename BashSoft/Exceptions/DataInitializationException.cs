using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class DataInitializationException : Exception
    {
        private const string IsInitializedMessage = "Data is already initialized!";
        private const string IsNotInitializedMessage = "The data structure must be initialised first in order to make any operations with it.";

        /// <summary>
        /// Display the default message according to whether the data is or is not initialized
        /// </summary>
        /// <param name="dataIsInitialized"></param>
        public DataInitializationException(bool dataIsInitialized) : base(dataIsInitialized ? IsInitializedMessage : IsNotInitializedMessage) { }

        public DataInitializationException(string message) : base(message) { }
    }
}
