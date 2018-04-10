﻿using BashSoft.Contracts;
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

        protected Command(string input, string[] data)
        {
            this.Input = input;
            this.Data = data;
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

        public void DisplayInvalidCommandMessage(string input) => OutputWriter.DisplayException($"The command '{input}' is invalid");

        public abstract void Execute();
    }
}
