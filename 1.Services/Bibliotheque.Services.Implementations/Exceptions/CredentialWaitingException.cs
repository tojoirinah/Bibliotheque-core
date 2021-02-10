using System;

namespace Bibliotheque.Services.Implementations.Exceptions
{
    public class CredentialWaitingException : Exception
    {
        private const string error = "This user is awaiting validation";

        public CredentialWaitingException() : base(error)
        {

        }
    }
}
