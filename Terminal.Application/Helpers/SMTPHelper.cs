using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Application.Helpers
{
    public class SMTPHelper
    {
        public static string SendRegistrationToken(SMTPConfiguration sender, string to, string context)
        {
            try
            {
                MailMessage message = new(sender.From, to)
                {
                    Subject = "Registration Token",
                    Body = $"Your Registration Token Is: {context}"
                };
                SmtpClient client = new(sender.Host)
                {
                    Port = sender.Port,
                    Credentials = new System.Net.NetworkCredential(sender.From, sender.Key)
                };
                client.Send(message);
                return $"[{DateTime.UtcNow.AddHours(4)} UTC + 4 ] Registration token sent successfuly towards {to}!";
            }
            catch (Exception ex)
            {
                return $"[{DateTime.UtcNow.AddHours(4)} UTC + 4 ] Something went wrong while sending registration token. Details:\nRecipient: {to}\nException: {ex}\nException Message: {ex.Message}\n\n";
            }
        }

        public static string SendPasswordRecoveryToken(SMTPConfiguration sender, string to, string context)
        {
            try
            {
                MailMessage message = new(sender.From, to)
                {
                    Subject = "Password Recovery Token",
                    Body = $"Your Password Recovery Token Is: {context}"
                };
                SmtpClient client = new(sender.Host)
                {
                    Port = sender.Port,
                    Credentials = new System.Net.NetworkCredential(sender.From, sender.Key)
                };
                client.Send(message);
                return $"[{DateTime.UtcNow.AddHours(4)} UTC + 4 ] Password recovery token sent successfuly towards {to}!";
            }
            catch (Exception ex)
            {
                return $"[{DateTime.UtcNow.AddHours(4)} UTC + 4 ] Something went wrong while sending password recovery token. Details:\nRecipient: {to}\nException: {ex}\nException Message: {ex.Message}\n\n";
            }
        }
    }
}
