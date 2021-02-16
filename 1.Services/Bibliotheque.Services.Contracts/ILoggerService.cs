using System;
using System.Collections.Generic;
using System.Text;

namespace Bibliotheque.Services.Contracts
{
    public interface ILoggerService
    {
        void SetError(string error);
        void SetDebug(string text);
    }
}
