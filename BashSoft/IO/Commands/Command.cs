using BashSoft.Exceptions;
using BashSoft.SimpleJudge;
using BashSoft.StudentRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public abstract class Command
    {
        private string input;
        private string[] data;
        private Tester judge;
        private StudentsRepository repository;
        private IOManager inputOutputManager;

        public Command(string input, string[] data, Tester judge, StudentsRepository repository, IOManager iOManager)
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

        protected Tester Judge => this.judge;

        protected StudentsRepository Repository => this.repository;

        protected IOManager InputOutputManager => this.inputOutputManager;

        public void DisplayInvalidCommandMessage(string input) => OutputWriter.DisplayException($"The command '{input}' is invalid");

        public abstract void Execute();
    }
}
