using HelloWorld.Contexts;
using HelloWorld.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Repositories;

public class CategoryRepository : ICategoryRepository {
	private readonly MainContext _context;

	public CategoryRepository(MainContext context) {
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public Task<bool> CategoryExistsAsync(int categoryID) {
		return _context.Categories.AnyAsync(c => c.ID == categoryID);
	}

	public async Task<IEnumerable<Category>> GetCategoriesAsync() {
		return await _context.Categories.ToListAsync();
	}

	public async Task<IEnumerable<Category>> GetCategoriesAsync(string? name) {
		IQueryable<Category> categories =
			_context.Categories as IQueryable<Category>;

		if (!string.IsNullOrWhiteSpace(name)) {
			name = name.Trim();
			categories = categories.Where(c => c.Name == name);
		}

		return await categories.ToListAsync();
	}

	public async Task<Category?> GetCategoryAsync(int id, bool includeProducts) {
		if (includeProducts) {
			return await _context.Categories.
									Include(c => c.Products).
									Where(c => c.ID == id).
									FirstOrDefaultAsync();
		}

		return await _context.Categories.
								Where(c => c.ID == id).
								FirstOrDefaultAsync();
	}


}
