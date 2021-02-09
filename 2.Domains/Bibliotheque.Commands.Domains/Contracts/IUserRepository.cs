using System;
using System.Collections.Generic;
using System.Text;

using Bibliotheque.Commands.Domains.Entities;

namespace Bibliotheque.Commands.Domains.Contracts
{
    public interface IUserRepository : IRepository<long, User>
    {
    }
}
