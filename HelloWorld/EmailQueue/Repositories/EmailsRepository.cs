using EmailQueue.Contexts;
using EmailQueue.Entities;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EmailQueue.Repositories;

public class EmailsRepository : IEmailsRepository {
	private readonly EmailContext _context;

	public EmailsRepository(EmailContext context) {
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<EmailMessage>> GetPendingEmails() {
		return await _context.EmailMessages.
			Where(e => !e.Sent).
			OrderByDescending(e => e.Created).
			ToListAsync();
	}

	public async Task AddEmail(EmailMessage message) {
		_context.EmailMessages.Add(message);
		await _context.SaveChangesAsync();
	}

	public async Task MarkAsSent(int id) {
		EmailMessage? message = await _context.
			EmailMessages.
			FindAsync(id);

		if (message == null) {
			return;
		}
		message.Sent = true;
		await _context.SaveChangesAsync();
	}
}
