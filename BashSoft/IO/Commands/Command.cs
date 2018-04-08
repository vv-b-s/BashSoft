using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public abstract class Command : IExecutable
    {
        private string input;
        private string[] data;
        private IContentComparer judge;
        private IStudentsRepository repository;
        private IDirectoryManager inputOutputManager;

        protected Command(string input, string[] data, IContentComparer judge, IStudentsRepository repository, IDirectoryManager iOManager)
        {
            this.Input = input;
            this.Data = data;
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = iOManager;
        }

        public string Input
        {
            get => this.input;
            private set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidStringException();

                this.input = value;
            }
        }

        public string[] Data
        {
            get => this.data;
            private set
            {
                if (value is null || value.Length == 0)
                    throw new NullReferenceException();

                this.data = value;
            }
        }

        protected IContentComparer Judge => this.judge;

        protected IStudentsRepository Repository => this.repository;

        protected IDirectoryManager InputOutputManager => this.inputOutputManager;

        public void DisplayInvalidCommandMessage(string input) => OutputWriter.DisplayException($"The command '{input}' is invalid");

        public abstract void Execute();
    }
}
