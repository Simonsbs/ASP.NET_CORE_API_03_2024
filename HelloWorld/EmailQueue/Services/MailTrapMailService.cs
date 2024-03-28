using System.Net.Mail;
using System.Net;
using EmailQueue.Entities;

namespace EmailQueue.Services;

public class MailTrapMailService : IMailService {
	public async Task SendAsync(EmailMessage message) {
		var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525) {
			Credentials = new NetworkCredential(
				"2e6a09ee00ce24", 
				"8c49fe4c2d99d8"
			),
			EnableSsl = true
		};
		await client.SendMailAsync(
			message.From, 
			message.To, 
			message.Subject, 
			message.Body);
	}
}