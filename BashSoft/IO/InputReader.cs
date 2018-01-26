using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Text;

using static BashSoft.IO.OutputWriter;


namespace BashSoft.IO
{
    public static class InputReader
    {
        /// <summary>
        /// Starts reading commands form the keyboard and sends them to the Interpreter
        /// </summary>
        public static void StartReadingCommands()
        {
            //Keep interpreting commands while the program is running
            while (true)
            {
                WriteMessage($"{SessionData.CurrentPath}> ");
                var input = Console.ReadLine().Trim();

                //If InterpredCommand returns false, it means the program should stop
                var stopLoop = !CommandInterpreter.InterpredCommand(input);

                if (stopLoop)
                    break;
            }
        }
    }
}
