using EmailQueue.Repositories;
using Microsoft.OpenApi.Writers;

namespace EmailQueue.Services;

public class EmailBackgroundService : BackgroundService {
	private readonly IServiceProvider _provider;

	public EmailBackgroundService(IServiceProvider provider) {
		_provider = provider ?? throw new ArgumentNullException(nameof(provider));

	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		while (!stoppingToken.IsCancellationRequested) {
			using (var scope = _provider.CreateScope()) {
				var repo = scope.ServiceProvider.GetRequiredService<IEmailsRepository>();
				var mail = scope.ServiceProvider.GetRequiredService<IMailService>();

				var emails = await repo.GetPendingEmails();

				foreach (var email in emails) {
					await mail.SendAsync(email);
					await repo.MarkAsSent(email.ID);
				}
			}

			await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
		}
	}
}
