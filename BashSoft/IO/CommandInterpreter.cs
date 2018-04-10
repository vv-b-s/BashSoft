using BashSoft.StaticData;
using BashSoft.StudentRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using BashSoft.SimpleJudge;
using BashSoft.Exceptions;
using BashSoft.IO.Commands;
using BashSoft.Contracts;
using System.Reflection;
using BashSoft.Attributes;
using System.Linq;
using DI_IoC;

namespace BashSoft.IO
{
    public class CommandInterpreter : IInterpreter
    {
        private DependencyContainer container;

        public CommandInterpreter(IContentComparer judge, IStudentsRepository repository, IDirectoryManager IOManager)
        {
            this.container = new DependencyContainer(typeof(InjectAttribute));
            container.AddDependency<IContentComparer, IContentComparer>(judge);
            container.AddDependency<IStudentsRepository, IStudentsRepository>(repository);
            container.AddDependency<IDirectoryManager, IDirectoryManager>(IOManager);
        }

        /// <summary>
        /// Interprets a command sent form the input. If the command is 'quit' will return 'false'. For any other command returns 'true'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool InterpredCommand(string input)
        {
            var data = input.Split();
            var command = data[0];

            if (command == "quit")
                return false;

            try
            {
                var commandObject = ParseCommand(input, command, data);
                commandObject.Execute();
            }
            catch (Exception ex) { OutputWriter.DisplayException(ex.Message); }

            return true;
        }

        private IExecutable ParseCommand(string input, string command, string[] data)
        {
            var parametersForConstruction = new object[] { input, data };

            var typeOfCommand = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes().Any(a => a.GetType() == typeof(AliasAttribute)))
                .FirstOrDefault(type => type.GetCustomAttribute<AliasAttribute>(false).Equals(command));

            var commandInstance = container.CreateAndInject(typeOfCommand, parametersForConstruction) as IExecutable;

            return commandInstance;

        }
    }
}
