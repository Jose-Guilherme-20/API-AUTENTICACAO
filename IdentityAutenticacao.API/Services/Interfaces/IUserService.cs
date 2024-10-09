using Microsoft.AspNetCore.Identity;

namespace IdentityAutenticacao.API.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(string email, string password);
}