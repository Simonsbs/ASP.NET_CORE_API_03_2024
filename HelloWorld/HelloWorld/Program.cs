using System.Text;
using HelloWorld.Contexts;
using HelloWorld.Controllers;
using HelloWorld.Repositories;
using HelloWorld.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace HelloWorld;
public class Program {
	public static void Main(string[] args) {
		Log.Logger = new LoggerConfiguration().
			MinimumLevel.Debug().
			WriteTo.Console().
			WriteTo.File("logs/shoplog.txt", rollingInterval: RollingInterval.Day).
			CreateLogger();

		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Host.UseSerilog();

		builder.Services.AddProblemDetails(op => {
			op.CustomizeProblemDetails = ctx => {
				ctx.ProblemDetails.Extensions.Add("SomeData", "This is some data");
				ctx.ProblemDetails.Extensions.Add("MachineName", Environment.MachineName);
			};
		});

		builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
		builder.Services.AddTransient<IMailService, LocalMailService>();
#else
        builder.Services.AddTransient<IMailService, RealMailService>();
#endif

		builder.Services.AddScoped<IProductRepository, ProdcutRepository>();
		builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
		builder.Services.AddScoped<IUserRepository, UserRepository>();

		builder.Services.AddDbContext<MainContext>(
			opt => opt.UseSqlite(builder.Configuration["ConnectionStrings:Main"])
		);

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		builder.Services.AddAuthentication("Bearer")
			.AddJwtBearer(o => {
				o.TokenValidationParameters = new() {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = builder.Configuration["Authentication:Issuer"],
					ValidAudience = builder.Configuration["Authentication:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.
						GetBytes(builder.Configuration["Authentication:SecretKey"])
					)
				};
			});

		builder.Services.AddAuthorization(o => {
			o.AddPolicy("IsAdmin", p => {
				p.RequireAuthenticatedUser();
				p.RequireClaim("auth", "10");
			});
		});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		//if (app.Environment.IsDevelopment()) {
		app.UseSwagger();
		app.UseSwaggerUI();
		//}

		app.UseHttpsRedirection();

		app.UseAuthentication();

		app.UseAuthorization();

		app.MapControllers();

		//app.Run(async (ctx) => {
		//    await ctx.Response.WriteAsync("Hello From Simon");
		//});

		app.Run();
	}
}