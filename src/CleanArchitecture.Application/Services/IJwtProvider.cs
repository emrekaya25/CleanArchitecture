using CleanArchitecture.Domain.AppUsers;

namespace CleanArchitecture.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user,CancellationToken cancellationToken=default);
}

