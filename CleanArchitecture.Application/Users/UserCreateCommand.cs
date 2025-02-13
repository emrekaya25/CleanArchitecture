using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Users;
using Mapster;
using MediatR;

namespace CleanArchitecture.Application.Users;

public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    DateTimeOffset? BirthOfDate,
    PersonalInformation? PersonalInformation,
    Address? Address) : IRequest<Result<string>>;

internal sealed class UserCreateCommandHandler(IUserRepository userRepository,IUnitOfWork unitOfWork) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        User user = request.Adapt<User>(); // Mapster ile mapleme işlemi.
        userRepository.Add(user);
        //AddAsync kullanmadık çünkü async ekleme işlemi için uygulama performansını azaltıyor.
        //!!!!(Ekleme sync kayıt işlemi async olmalı.)
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı kaydı başarıyla tamamlandı.";
    }
}