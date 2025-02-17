using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users;

public sealed record UserGetAllQuery() : IRequest<IQueryable<UserGetAllQueryResponse>>;

public sealed class UserGetAllQueryResponse: EntityDTO
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string TcNo { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
}

internal sealed class UserGetAllQueryHandler(IUserRepository userRepository) : IRequestHandler<UserGetAllQuery, IQueryable<UserGetAllQueryResponse>>
{
    public Task<IQueryable<UserGetAllQueryResponse>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = userRepository.GetAll()
            .Select(x => new UserGetAllQueryResponse
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                TcNo = x.PersonalInformation.TcNo,
                FullAddress = x.Address.FullAddress,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                DeleteAt = x.DeleteAt,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            })
            .AsQueryable();
        //Memory'e çekmiyoruz verileri , AsQueryable db tarafında yapılıyor.
        //veriler db den geldiği için mapleyemiyoruz.
        return Task.FromResult(response);
    }
}