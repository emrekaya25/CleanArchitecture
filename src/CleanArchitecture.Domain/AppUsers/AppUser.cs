using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.AppUsers;
public sealed class AppUser : IdentityUser<Guid> // verdiğim guid Id'sinin db de tutulma tipi
{
    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }
    // diğer çoğu alan zaten IdentityUser kütüphanesinde var.
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}"; // computed property olarak geçiyor.

    //*** Normalde Repository'leride yazmamız lazım fakat çok spesifik bir şeye ihtiyacımız yoksa IdentityUser'dan gelen UserManager kütüphanesi çoğu şeyi karşılıyor.(Lazım olursa eklenir.)

    #region Audit Log
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
