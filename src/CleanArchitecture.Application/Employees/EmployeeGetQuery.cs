using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Employees;
using MediatR;

namespace CleanArchitecture.Application.Employees;
public sealed record EmployeeGetQuery(
    Guid Id) : IRequest<Result<Employee>>;

internal sealed class EmployeeGetQueryHandler(
    IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetQuery, Result<Employee>>
{
    public async Task<Result<Employee>> Handle(EmployeeGetQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (employee is null)
        {
            return Result<Employee>.Failure("Kullanıcı bulunamadı");
        }

        return employee;
    }
}
