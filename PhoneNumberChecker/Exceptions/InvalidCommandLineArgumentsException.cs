using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sitel.Applications.PhoneNumberChecker.Exceptions
{
    public class InvalidCommandLineArgumentsException : Exception
    {
        public InvalidCommandLineArgumentsException(string parameter) : base($"Unknown option \"{parameter}\" in command line.")
        {
            Log.Error(base.Message);
        }
    }
}
