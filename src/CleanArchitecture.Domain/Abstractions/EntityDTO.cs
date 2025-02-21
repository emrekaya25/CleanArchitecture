using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstractions;

public abstract class EntityDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public Guid CreateUserId { get; set; }
    public string CreateUserName { get; set; } = default!;
    public DateTimeOffset UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public string? UpdateUserName { get; set; }
    public DateTimeOffset DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    public string? DeleteUserName { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}

