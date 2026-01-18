using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace NetshopRazor.MyHelpers
{
	public class EmailSender
	{
		public static async Task SendEmail(
			string toEmail,
			string username,
			string subject,
			string message
		)
		{
			var email = new MimeMessage();

			email.From.Add(new MailboxAddress(
				"Pandji Asmoro",
				"NetshopRazor@gmail.com"
			));

			email.To.Add(new MailboxAddress(username, toEmail));
			email.Subject = subject;

			email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
			{
				Text = message
			};

			using var smtp = new SmtpClient();

			await smtp.ConnectAsync(
				"sandbox.smtp.mailtrap.io",
				2525,
				SecureSocketOptions.StartTls
			);

			await smtp.AuthenticateAsync("xxx", "aaa");

			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}

