using System;

namespace Bibliotheque.Services.Implementations.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        private const string error = "User already exist";

        public UserAlreadyExistsException() : base(error)
        {

        }
    }
}
