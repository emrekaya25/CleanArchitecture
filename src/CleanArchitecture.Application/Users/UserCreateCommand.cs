using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Users;
using FluentValidation;
using Mapster;
using MediatR;

namespace CleanArchitecture.Application.Users;

//DTO nesnesi
public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    DateTimeOffset? BirthOfDate,
    PersonalInformation PersonalInformation,
    Address? Address) : IRequest<Result<string>>;

//Validation(**Public olmazsa çalışmaz.)
public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x=>x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.PersonalInformation.TcNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC numarası yazınız.")
            .MaximumLength(11).WithMessage("Geçerli bir TC numarası yazınız.");
    }
}

//Add işlemi
internal sealed class UserCreateCommandHandler(IUserRepository userRepository,IUnitOfWork unitOfWork) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = await userRepository.AnyAsync(x => x.PersonalInformation.TcNo == request.PersonalInformation.TcNo, cancellationToken);

        if (isUserExist)
        {
            return Result<string>.Failure("Bu TC numarası daha önce kaydedilmiş");
        }

        User user = request.Adapt<User>(); // Mapster ile mapleme işlemi.
        userRepository.Add(user);
        //AddAsync kullanmadık çünkü async ekleme işlemi için uygulama performansını azaltıyor.
        //!!!!(Ekleme sync kayıt işlemi async olmalı.)
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı kaydı başarıyla tamamlandı.";
        //result sınıfımda implicit olduğu için fast return atabildim.
    }
}