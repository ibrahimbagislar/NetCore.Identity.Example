using System.Net;
using System.Net.Mail;

namespace Identity.ExampleUdemy.Services
{
    public class MailService : IMailService
    {
        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true,string userName = "", string confirmMessage = "")
        {
            await SendMessageAsync(new[] { to }, subject, body, isBodyHtml,userName,confirmMessage);
        }

        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true,string userName = "", string confirmMessage = "")
        {

            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;

            foreach (var to in tos)
            {
                mail.To.Add(to);
            }
            mail.Subject = subject;
            mail.Body = new MailFrontend().MailBody(userName,confirmMessage);
            mail.From = new("info@ibrahimbagislar.com","IDENTİTYAPP",System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential("info@ibrahimbagislar.com","!ibraQim19.");
            smtp.Port = 587;
            smtp.EnableSsl = false;
            smtp.Host = "mt-engine-win.guzelhosting.com";
                await smtp.SendMailAsync(mail);
        }
    }
}
