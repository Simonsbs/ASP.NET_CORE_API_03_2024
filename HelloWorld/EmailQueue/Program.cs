
using EmailQueue.Contexts;
using EmailQueue.Repositories;
using EmailQueue.Services;
using Microsoft.EntityFrameworkCore;

namespace EmailQueue;

public class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddDbContext<EmailContext>(o =>
			o.UseSqlite(builder.
			Configuration["ConnectionStrings:Main"])
		);

		builder.Services.AddScoped<IEmailsRepository, EmailsRepository>();

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		builder.Services.AddTransient<IMailService, MailTrapMailService>();

		builder.Services.AddHostedService<EmailBackgroundService>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}
