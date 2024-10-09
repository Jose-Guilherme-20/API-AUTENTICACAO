
using IdentityAutenticacao_DOMAIN.Models;
using IdentityAutenticacao.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityAutenticacao.API.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IdentityResult> CreateUserAsync(string email, string password)
    {
        var user = new ApplicationUser() { UserName = email, Email = email };
        return await _userManager.CreateAsync(user, password);
    }
}