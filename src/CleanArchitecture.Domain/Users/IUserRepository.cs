using CleanArchitecture.Domain.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Users;

public interface IUserRepository : IRepository<User>
{
}

