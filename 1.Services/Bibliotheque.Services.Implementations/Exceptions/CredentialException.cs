using System;

namespace Bibliotheque.Services.Implementations.Exceptions
{
    public class CredentialException : Exception
    {
        private const string error = "Wrong password";

        public CredentialException():base(error)
        {

        }
    }
}
