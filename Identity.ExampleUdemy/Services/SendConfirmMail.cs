using NetCore.Identity.Example.Data.Entites;
using NetCore.Identity.Example.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NetCore.Identity.Example.Services
{
    public class SendConfirmMail
    {
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;
        public SendConfirmMail(IMailService mailService, UserManager<AppUser> userManager)
        {
            _mailService = mailService;
            _userManager = userManager;
        }
        public async Task SendAsync(AppUser appUser)
        {
            var user = appUser;
            var checkEndDate = user.ConfirmCodeEndDate - DateTime.UtcNow;
            if (checkEndDate.TotalSeconds < 0)
            {
                Random rnd = new();
                user.ConfirmCodeEndDate = DateTime.UtcNow.AddMinutes(3);
                user.ConfirmCode = rnd.Next(100000, 1000000);
                await _userManager.UpdateAsync(user);
            }
            var confirmMessage = "Email doğrulama kodunuz: " + user.ConfirmCode;
            await _mailService.SendMessageAsync(user.Email, "HOŞGELDİN " + user.UserName, "IDENTITYAPP'E HOŞGELDİN " + user.UserName, true, user.UserName, confirmMessage);
        }
    }
}
