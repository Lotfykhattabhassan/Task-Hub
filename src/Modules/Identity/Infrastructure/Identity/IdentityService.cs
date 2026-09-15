using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;
using TaskHub.Modules.Identity.Application.Exceptions;
using TaskHub.Modules.Identity.Domain.Entities;
using TaskHub.Modules.Identity.Domain.Exceptions;
using TaskHub.Modules.Identity.Domain.ValueObjects;
using TaskHub.Modules.Identity.Infrastructure.Persistence;

namespace TaskHub.Modules.Identity.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IdentityDbContext _dbContext;
    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IdentityDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken =default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
   

    }


    public async Task<IdentityOperationResult> CreateUserAsync(
    User user,
    string phoneNumber,
    string password)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException(
                "Password is required.",
                nameof(password));

        var applicationUser = new ApplicationUser
        {
            Id = user.Id,
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.Email.Value,
            PhoneNumber = phoneNumber
        };

        // Add the Domain User first.
        _dbContext.Users.Add(user);

        var result = await _userManager.CreateAsync(
            applicationUser,
            password);

        return new IdentityOperationResult(
            result.Succeeded,
            result.Errors
                .Select(error => error.Description)
                .ToArray());
    }

    public async Task<AuthenticatedUser?> ValidateCredentialsAsync(
    Email email,
    string password)
    {
        var user = await _userManager.FindByEmailAsync(email.Value);

        if (user is null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true);

        if (!result.Succeeded)
            return null;

        return new AuthenticatedUser(
            user.Id,
            user.Email!);
    }

}