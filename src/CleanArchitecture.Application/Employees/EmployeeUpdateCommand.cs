using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Employees;
using FluentValidation;
using Mapster;
using MediatR;

namespace CleanArchitecture.Application.Employees;

public sealed record EmployeeUpdateCommand(
    Guid Id,
    string? FirstName,
    string? LastName,
    DateTimeOffset? BirthOfDate,
    PersonalInformation PersonalInformation,
    Address? Address) : IRequest<Result<string>>;

public sealed class EmployeeUpdateCommandValidator : AbstractValidator<EmployeeUpdateCommand>
{
    public EmployeeUpdateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.PersonalInformation.TcNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC numarası yazınız.")
            .MaximumLength(11).WithMessage("Geçerli bir TC numarası yazınız.");
    }
}

internal sealed class EmployeeUpdateCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeUpdateCommand, Result<string>>
{
    async Task<Result<string>> IRequestHandler<EmployeeUpdateCommand, Result<string>>.Handle(EmployeeUpdateCommand request, CancellationToken cancellationToken)
    {
        Employee? employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

        if (employee is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        if (!string.IsNullOrWhiteSpace(request.PersonalInformation?.TcNo)) //TcNo boş "" girildi ise eski değerini geri alır.
        {
            employee.PersonalInformation.TcNo = request.PersonalInformation.TcNo;
        }
        request.Adapt(employee);
        employeeRepository.Update(employee);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarıyla güncellendi.";
    }
}

