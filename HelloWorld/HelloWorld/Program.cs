using System.Reflection;
using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using HelloWorld.Contexts;
using HelloWorld.Controllers;
using HelloWorld.Repositories;
using HelloWorld.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
						//Encoding.UTF8.
						//GetBytes(builder.Configuration["Authentication:SecretKey"])
						Convert.FromBase64String(builder.Configuration["Authentication:SecretKey"])
					)
				};
			});

		builder.Services.AddAuthorization(o => {
			o.AddPolicy("IsAdmin", p => {
				p.RequireAuthenticatedUser();
				p.RequireClaim("auth", "10");
			});
		});

		builder.Services.AddApiVersioning(o => {
			o.ReportApiVersions = true;
			o.AssumeDefaultVersionWhenUnspecified = true;
			o.DefaultApiVersion = new ApiVersion(1, 0);
		}).AddMvc()
		.AddApiExplorer(o => {
			o.SubstituteApiVersionInUrl = true;
		});

		var apiProvider = builder.Services.BuildServiceProvider()
			.GetRequiredService<IApiVersionDescriptionProvider>();

		builder.Services.AddSwaggerGen(o => {
			foreach (var desc in apiProvider.ApiVersionDescriptions) {
				o.SwaggerDoc($"{desc.GroupName}", new() {
					Title = "This is my cool API",
					Version = desc.ApiVersion.ToString(),
					Description = "This api is for getting categories and products"
				});
			}

			var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var path = Path.Combine(AppContext.BaseDirectory, file);

			o.IncludeXmlComments(path);

			o.AddSecurityDefinition("MyAPIAuth", new() {
				Type = SecuritySchemeType.Http,
				Scheme = "Bearer",
				Description = "Enter a valid token to access the API"
			});

			o.AddSecurityRequirement(new() {
				{
					new() {
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
							Id = "MyAPIAuth"
						}
					},
					new List<string>()
				}
			})
			;
		});


		var app = builder.Build();

		// Configure the HTTP request pipeline.
		//if (app.Environment.IsDevelopment()) {
		app.UseSwagger();
		app.UseSwaggerUI(o => {
			var descriptions = app.DescribeApiVersions();

			foreach (var desc in descriptions) {
				o.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
					desc.GroupName.ToUpper());
			}
		});
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