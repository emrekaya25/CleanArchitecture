using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.UserRoles;

public sealed class UserRole : Entity
{
    public Guid UserId { get; set; } = default!;
    public Guid RoleId { get; set; } = default!;
}
