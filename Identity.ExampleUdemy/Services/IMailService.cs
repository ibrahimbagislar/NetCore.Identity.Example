namespace Identity.ExampleUdemy.Services
{
    public interface IMailService
    {
        Task SendMessageAsync(string to,string subject, string body,bool isBodyHtml = true,string userName = "");
        Task SendMessageAsync(string[] tos,string subject, string body,bool isBodyHtml = true, string userName = "");
    }
}
