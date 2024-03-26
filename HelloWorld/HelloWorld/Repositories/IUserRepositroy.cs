using HelloWorld.Entities;

namespace HelloWorld.Repositories;

public interface IUserRepository {
	Task<User?> GetUser(string username, string password);
}
