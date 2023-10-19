# NetCore.Identity.Example

Bu proje, .NET Core Identity kullanarak bir kimlik doğrulama ve yetkilendirme örneğidir.

## Başlarken

Bu proje kullanılmadan önce aşağıdaki adımları takip ederek gerekli ayarlamaları yapmalısınız.

### 1. Uygulama Ayarları

Proje, `appsettings.json` dosyasında bazı temel ayarları içerir. Bu ayarları kendi projenize uygun şekilde özelleştirmeniz gerekmektedir. Veritabanı bağlantı dizisini kendi veritabanınıza uygun şekilde değiştirmeniz gerekir.

Ayarlar:

- `ConnectionStrings:MsSQL`: Veritabanı bağlantı dizesini belirleyin. Örneğin, `Server=localhost;Database=IdentityDb;User Id=yourusername;Password=yourpassword;` şeklinde olabilir.

### 2. Email Ayarları

Proje, kullanıcılara çeşitli durumlarda e-posta gönderme işlevselliğini içerir. Bu e-postaları gönderebilmek için bir SMTP sunucusu ayarlamalısınız.

Ayarlar:

- `SmtpServer`: SMTP sunucu adresini belirleyin.
- `Port`: SMTP sunucu bağlantı noktasını belirleyin.
- `UserName`: SMTP sunucu kullanıcı adınızı belirleyin.
- `Password`: SMTP sunucu şifrenizi belirleyin.
- `FromAddress`: E-postaların "Kimden" adresini belirleyin.


