using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HelloWorld.Entities;
using HelloWorld.Models;
using HelloWorld.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase {
	private ILogger<LoginController> _logger;
	private readonly IUserRepository _repo;
	private readonly IMapper _mapper;
	private IConfiguration _config;

	public LoginController(
		ILogger<LoginController> logger,
		IUserRepository repo,
		IMapper mapper,
		IConfiguration config
		) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_config = config ?? throw new ArgumentNullException(nameof(config));
	}

	[HttpPost]
	public async Task<ActionResult<string>> Login(LoginRequestDTO login) {
		User? user = await _repo.GetUser(login.Username, login.Password);

		if (user == null) {
			return Unauthorized();
		}

		var key = new SymmetricSecurityKey(
			//Convert.FromBase64String(_config["Authentication:SecretKey"])
			Encoding.UTF8.
			GetBytes(_config["Authentication:SecretKey"])
		);

		var creds = new SigningCredentials(
			key,
			SecurityAlgorithms.HmacSha256
		);

		var token = new JwtSecurityToken(
			_config["Authentication:Issuer"],
			_config["Authentication:Audience"],
			new List<Claim>() {
				new Claim ("sub", user.ID.ToString()),
				new Claim("auth", user.AutherizationLevel.ToString()),
				new Claim("user_name", user.Username),
				// new Claim("password", user.Password) // DONT DO THIS!!!!
				new Claim("allowed_category", "1")
			},
			DateTime.UtcNow,
			DateTime.UtcNow.AddHours(1),
			creds
		);

		var tokenString = new JwtSecurityTokenHandler().
			WriteToken(token);

		return Ok(tokenString);
	}
}