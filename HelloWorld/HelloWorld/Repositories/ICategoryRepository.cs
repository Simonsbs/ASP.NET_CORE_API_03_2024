using HelloWorld.Entities;

namespace HelloWorld.Repositories;

public interface ICategoryRepository {
	Task<IEnumerable<Category>> GetCategoriesAsync();
	Task<Category?> GetCategoryAsync(int id, bool includeProducts);
}
