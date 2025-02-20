using CleanArchitecture.Application.Users;
using CleanArchitecture.Domain.Users;
using Mapster;

namespace CleanArchitecture.Infrastructure.Mapping
{
    public sealed class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserUpdateCommand, User>()
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
