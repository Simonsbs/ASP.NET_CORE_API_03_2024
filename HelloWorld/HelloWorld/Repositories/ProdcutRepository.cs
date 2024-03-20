using HelloWorld.Contexts;
using HelloWorld.Entities;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace HelloWorld.Repositories;

public class ProdcutRepository : IProductRepository {
	private readonly MainContext _context;

	public ProdcutRepository(MainContext context) {
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Product>> GetProductsAsync() {
		return await _context.Products.ToListAsync();
	}

	public async Task<IEnumerable<Product>> GetProductsForCategoryAsync(int categoryID) {
		return await _context.Products.Where(p => p.CategoryID == categoryID).ToListAsync();
	}

	public Task AddProductAsync(Product product) {
		throw new NotImplementedException();
	}

	public Task DeleteProductAsync(Product product) {
		throw new NotImplementedException();
	}

	public async Task<bool> SaveAsync() {
		return await _context.SaveChangesAsync() >= 0;
	}
}
