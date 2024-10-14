using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;

using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PackProApp.Services.MailServices
{
    public class MailService : IMailService
    {

        private readonly Email _email;

        public MailService(IOptions<Email> email) => _email = email.Value;





        public async Task SendMailAsync(string email, string subject, string message)
        {
            try
            {
                var newEmail = new MimeMessage();
                newEmail.From.Add(MailboxAddress.Parse("urremzi@gmail.com"));
                newEmail.To.Add(MailboxAddress.Parse(email));
                newEmail.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = message;
                newEmail.Body = builder.ToMessageBody();

                var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("urremzi@gmail.com", "xkuwrlzxbznrygha");
                await smtp.SendAsync(newEmail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"E-posta gonderilirken bir hata oluştur :" + ex.Message);
            }
        }
    }
}
