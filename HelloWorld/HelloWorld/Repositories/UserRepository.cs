using HelloWorld.Contexts;
using HelloWorld.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Repositories;

public class UserRepository : IUserRepository {
	private readonly MainContext _context;

	public UserRepository(MainContext context) {
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<User?> GetUser(string username, string password) {
		return await _context.Users.FirstOrDefaultAsync(
			u => u.Username == username && 
			u.Password == password
		);
	}
}
