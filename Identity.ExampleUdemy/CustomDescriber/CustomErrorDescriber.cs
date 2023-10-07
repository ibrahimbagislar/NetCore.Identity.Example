using NetCore.Identity.Example.Data.Entites;
using Microsoft.AspNetCore.Identity;

namespace NetCore.Identity.Example.CustomDescriber
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new()
            {
                Description = "Şifre sayı içermelidir.",
                Code = "PasswordRequiresDigit"
            };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new()
            {
                Description = "Şifre en az 1 adet büyük harf(A-Z) içerleidir.",
                Code = "PasswordRequiresUpper"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Description = $"Şifre en az {length} karakter uzunluğunda olmalıdır.",
                Code = "PasswordTooShort"
            };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new()
            {
                Code = "PasswordRequiresLower",
                Description = "Şifre en az bir küçük harf(a-z) içermelidir."
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new()
            {
                Code = "DuplicateUserName",
                Description = $"{userName} zaten alınmış."
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new()
            {
                Code = "DuplicateEmail",
                Description = $"{email} zaten alınmış."
            };
        }
        public override IdentityError InvalidRoleName(string role)
        {
                return new()
                {
                    Code = "InvalidRoleName",
                    Description = "Geçersiz rol ismi."
                };
        }
        public override IdentityError DuplicateRoleName(string role)
        {
            return new()
            {
                Code = "DuplicateRoleName",
                Description = $"{role} ismi zaten alınmış."
            };
        }
        public override IdentityError PasswordMismatch()
        {
            return new()
            {
                Code = "PasswordMismatch",
                Description = "Mevcut şifre doğru değil."
            };
        }
        public override IdentityError UserNotInRole(string role)
        {
            return new()
            {
                Code = "UserNotInRole",
                Description = $"Seçilen kullanıcı da '{role}' rolü zaten yok."
            };
        }
        public override IdentityError UserAlreadyInRole(string role)
        {
            return new()
            {
                Code = "UserAlreadyInRole",
                Description = $"Seçilen kullanıcı da '{role}' rolü zaten var."
            };
        }
    }
}
