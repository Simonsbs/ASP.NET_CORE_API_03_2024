using HelloWorld.Entities;

namespace HelloWorld.Repositories;

public interface IProductRepository {
	Task<IEnumerable<Product>> GetProductsAsync();

	Task<IEnumerable<Product>> GetProductsForCategoryAsync(int categoryID);

	Task<Product?> GetProductForCategoryAsync(int categoryID, int productID);

	Task AddProductAsync(Product product, bool autosave = true);

	Task DeleteProductAsync(Product product);

	Task<bool> SaveAsync();

	

}
