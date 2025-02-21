using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Employees;
using MediatR;

namespace CleanArchitecture.Application.Employees;

public sealed record EmployeeDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class EmployeeDeleteCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeDeleteCommand request, CancellationToken cancellationToken)
    {
        Employee? user = await employeeRepository.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }
        user.IsDeleted = true;
        employeeRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return $"{user.FullName} isimli kullanıcı başarıyla silindi.";
    }
}

