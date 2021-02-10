using System;

namespace Bibliotheque.Services.Implementations.Exceptions
{
    public class UknownOrDisabledUserException : Exception
    {
        private const string error = "Uknown or disabled user";

        public UknownOrDisabledUserException() : base(error)
        {

        }
    }
}
