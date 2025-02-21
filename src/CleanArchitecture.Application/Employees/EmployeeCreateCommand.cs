using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Employees;
using FluentValidation;
using Mapster;
using MediatR;

namespace CleanArchitecture.Application.Employees;

//DTO nesnesi
public sealed record EmployeeCreateCommand(
    string FirstName,
    string LastName,
    DateTimeOffset? BirthOfDate,
    PersonalInformation PersonalInformation,
    Address? Address) : IRequest<Result<string>>;

//Validation(**Public olmazsa çalışmaz.)
public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x=>x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.PersonalInformation.TcNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC numarası yazınız.")
            .MaximumLength(11).WithMessage("Geçerli bir TC numarası yazınız.");
    }
}

//Add işlemi
internal sealed class EmployeeCreateCommandHandler(IEmployeeRepository employeeRepository,IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = await employeeRepository.AnyAsync(x => x.PersonalInformation.TcNo == request.PersonalInformation.TcNo, cancellationToken);

        if (isUserExist)
        {
            return Result<string>.Failure("Bu TC numarası daha önce kaydedilmiş");
        }

        Employee user = request.Adapt<Employee>(); // Mapster ile mapleme işlemi.
        employeeRepository.Add(user);
        //AddAsync kullanmadık çünkü async ekleme işlemi için uygulama performansını azaltıyor.
        //!!!!(Ekleme sync kayıt işlemi async olmalı.)
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı kaydı başarıyla tamamlandı.";
        //result sınıfımda implicit olduğu için fast return atabildim.
    }
}