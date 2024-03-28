
using EmailQueue.Entities;

namespace EmailQueue.Services;

public interface IMailService {
	Task SendAsync(EmailMessage message);
}