using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Users;
using FluentValidation;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users;

public sealed record UserUpdateCommand(
    Guid Id,
    string? FirstName,
    string? LastName,
    DateTimeOffset? BirthOfDate,
    PersonalInformation PersonalInformation,
    Address? Address) : IRequest<Result<string>>;

public sealed class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.PersonalInformation.TcNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC numarası yazınız.")
            .MaximumLength(11).WithMessage("Geçerli bir TC numarası yazınız.");
    }
}

internal sealed class UserUpdateCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    async Task<Result<string>> IRequestHandler<UserUpdateCommand, Result<string>>.Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        if (!string.IsNullOrWhiteSpace(request.PersonalInformation?.TcNo)) //TcNo boş "" girildi ise eski değerini geri alır.
        {
            user.PersonalInformation.TcNo = request.PersonalInformation.TcNo;
        }
        request.Adapt(user);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarıyla güncellendi.";
    }
}

