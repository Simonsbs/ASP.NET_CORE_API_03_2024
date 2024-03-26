using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
public class LoginController : ControllerBase {
	[HttpPost]
	public ActionResult<string> Login(LoginRequestDTO login) {


		return string.Empty;
	}
}
