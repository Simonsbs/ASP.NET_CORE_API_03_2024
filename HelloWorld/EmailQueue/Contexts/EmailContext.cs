using EmailQueue.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailQueue.Contexts;

public class EmailContext : DbContext {
	public EmailContext(DbContextOptions options) :
		base(options) {
	}

    public DbSet<EmailMessage> EmailMessages { get; set; }
}
