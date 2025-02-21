using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users;

public sealed record UserGetAllQuery() : IRequest<IQueryable<UserGetAllQueryResponse>>;

public sealed class UserGetAllQueryResponse : EntityDTO
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string TcNo { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
}

internal sealed class UserGetAllQueryHandler(IUserRepository userRepository,
    UserManager<AppUser> userManager) : IRequestHandler<UserGetAllQuery, IQueryable<UserGetAllQueryResponse>>
{
    public Task<IQueryable<UserGetAllQueryResponse>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        //bunlar inner join
        var response = (
                        from user in userRepository.GetAll()
                        // yeni oluşturulmada oluşturan kullanıcı kısmı
                        join create_user in userManager.Users.AsQueryable() on user.CreateUserId equals create_user.Id
                        // güncelleyen kullanıcı kısmı
                        join update_user in userManager.Users.AsQueryable() on user.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        // silen kullanıcı kısmı 
                        join delete_user in userManager.Users.AsQueryable() on user.DeleteUserId equals delete_user.Id into delete_user
                        from delete_users in delete_user.DefaultIfEmpty() // bu kısımda left join'e çevriliyor.
                        // güncelleyen ve silen kısmı boş olma ihtimali olduğu için ekstra defaultIfEmpty() sorgusunu ekledik.
                        select new UserGetAllQueryResponse
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            TcNo = user.PersonalInformation.TcNo,
                            FullAddress = user.Address.FullAddress,

                            CreateAt = user.CreateAt,
                            CreateUserId = user.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",

                            UpdateAt = user.UpdateAt,
                            UpdateUserId = user.UpdateUserId,
                            UpdateUserName = user.UpdateUserId == null ? null : update_users.FirstName  + " " + update_users.LastName + " (" + update_users.Email + ")",

                            DeleteAt = user.DeleteAt,
                            DeleteUserId = user.DeleteUserId,
                            DeleteUserName = user.DeleteUserId == null ? null : delete_users.FirstName + " " + delete_users.LastName + " (" + delete_users.Email + ")",

                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted
                        }).AsQueryable();
        //Memory'e çekmiyoruz verileri , AsQueryable db tarafında yapılıyor.
        //veriler db den geldiği için mapleyemiyoruz.
        return Task.FromResult(response);
    }
}