using System;

namespace Bibliotheque.Services.Implementations.Exceptions
{
    public class UserNotFoundException : Exception
    {
        private const string error = "User not found.";

        public UserNotFoundException() : base(error)
        {

        }
    }
}
