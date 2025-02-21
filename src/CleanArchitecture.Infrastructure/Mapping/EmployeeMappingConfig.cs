using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domain.Employees;
using Mapster;

namespace CleanArchitecture.Infrastructure.Mapping
{
    public sealed class EmployeeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EmployeeUpdateCommand, Employee>()
                .IgnoreNullValues(true);
            /*     .Map(dest => dest.PersonalInformation.TcNo,     
         src => string.IsNullOrWhiteSpace(src.PersonalInformation?.TcNo) 
                ? dest.PersonalInformation.TcNo 
                : src.PersonalInformation.TcNo);
            */
            //TcNo boş string gelirse eski değerini alır.
        }
    }
}
