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
		return await _context.Products.
			Where(p => p.CategoryID == categoryID).
			ToListAsync();
	}

	public Task<Product?> GetProductForCategoryAsync(int categoryID, int productID) {
		return _context.Products.
			Where(p => 
				p.CategoryID == categoryID && 
				p.ID == productID).
				FirstOrDefaultAsync();
	}

	public async Task AddProductAsync(Product product, 
		bool autosave = true) {
		await _context.Products.AddAsync(product);
		if (autosave) {
			await _context.SaveChangesAsync(); 
		}
	}

	public async Task DeleteProductAsync(Product product,
		bool autosave = true) {
		_context.Remove(product);
		if (autosave) {
			await _context.SaveChangesAsync();
		}
	}

	public async Task<bool> SaveAsync() {
		return await _context.SaveChangesAsync() >= 0;
	}


}
