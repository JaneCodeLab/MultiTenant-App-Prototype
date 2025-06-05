
using ApplicationCore.DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Presentation;

public class CustomUserManager : UserManager<SysCustomUser>
{
    public CustomUserManager(IUserStore<SysCustomUser> store,
                            IOptions<IdentityOptions> optionsAccessor,
                            IPasswordHasher<SysCustomUser> passwordHasher,
                            IEnumerable<IUserValidator<SysCustomUser>> userValidators,
                            IEnumerable<IPasswordValidator<SysCustomUser>> passwordValidators,
                            ILookupNormalizer keyNormalizer,
                            IdentityErrorDescriber errors,
                            IServiceProvider services,
                            ILogger<UserManager<SysCustomUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {

    }

    public override Task<IdentityResult> CreateAsync(SysCustomUser user)
    {
        //Todo : Check it
        user.UserName = user.Email;
        user.CreatedAt = DateTime.UtcNow;
        user.TwoFactorEnabled = true;

        return base.CreateAsync(user);
    }
}
