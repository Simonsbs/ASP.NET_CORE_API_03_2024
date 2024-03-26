using HelloWorld.Entities;
using HelloWorld.Models;

namespace HelloWorld.Repositories;

public interface ICategoryRepository {
	Task<IEnumerable<Category>> GetCategoriesAsync();

	Task<(IEnumerable<Category>, PagingMetaData)> GetCategoriesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

	Task<Category?> GetCategoryAsync(int id, bool includeProducts);
	Task<bool> CategoryExistsAsync(int categoryID);
}
