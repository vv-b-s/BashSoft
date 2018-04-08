using BashSoft.Contracts;
using BashSoft.StaticData;
using System;
using System.Collections.Generic;
using System.Text;


namespace BashSoft.IO
{
    public class InputReader : IReader
    {
        private IInterpreter interpreter;
        public InputReader(IInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        /// <summary>
        /// Starts reading commands form the keyboard and sends them to the Interpreter
        /// </summary>
        public void StartReadingCommands()
        {
            //Keep interpreting commands while the program is running
            while (true)
            {
                OutputWriter.WriteMessage($"{SessionData.CurrentPath}> ");
                var input = Console.ReadLine().Trim();

                //If InterpredCommand returns false, it means the program should stop
                var stopLoop = !interpreter.InterpredCommand(input);

                if (stopLoop)
                    break;
            }
        }
    }
}
