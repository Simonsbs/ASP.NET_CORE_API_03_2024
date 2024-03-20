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
}
