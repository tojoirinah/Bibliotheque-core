using System;

using Bibliotheque.Services.Contracts;

namespace Bibliotheque.Specs.Services
{
    public class LoggerService : ILoggerService
    {
        public void SetDebug(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            System.Diagnostics.Debug.WriteLine(text);
            Console.WriteLine(text);
        }

        public void SetError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Diagnostics.Debug.WriteLine(error);
            Console.WriteLine(error);
        }
    }
}
