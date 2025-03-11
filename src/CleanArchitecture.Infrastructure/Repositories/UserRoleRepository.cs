using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.UserRoles;
using CleanArchitecture.Infrastructure.Context;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class UserRoleRepository : Repository<UserRole, ApplicationDbContext>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
