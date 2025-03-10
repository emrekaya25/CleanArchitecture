using CleanArchitecture.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.AppRoles;

public sealed class AppRole: IdentityRole<Guid> , IEntity
{
    //inherit olduğu classtan Id,Name,NormalizedName barındırıyor.

    //inherit olduğu classtaki proplar virtual yani ben istediğimi db ye yazdırmam.
    //[NotMapped]
    //public override string? ConcurrencyStamp { get; set; }

    #region Audit
    public DateTimeOffset CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public DateTimeOffset DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    #endregion
}
