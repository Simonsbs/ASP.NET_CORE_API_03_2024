using HelloWorld.Entities;

namespace HelloWorld.Repositories;

public interface ICategoryRepository {
	Task<IEnumerable<Category>> GetCategoriesAsync();

	Task<IEnumerable<Category>> GetCategoriesAsync(string? name, string? searchQuery);

	Task<Category?> GetCategoryAsync(int id, bool includeProducts);
	Task<bool> CategoryExistsAsync(int categoryID);
}
