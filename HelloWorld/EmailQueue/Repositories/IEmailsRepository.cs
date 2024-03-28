using EmailQueue.Entities;

namespace EmailQueue.Repositories;
public interface IEmailsRepository {
	Task AddEmail(EmailMessage message);
	Task<IEnumerable<EmailMessage>> GetPendingEmails();
	Task MarkAsSent(int id);
}