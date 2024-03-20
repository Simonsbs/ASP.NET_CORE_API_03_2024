using HelloWorld.Entities;

namespace HelloWorld.Repositories;

public interface IProductRepository {
	Task<IEnumerable<Product>> GetProductsAsync();

	Task<IEnumerable<Product>> GetProductsForCategoryAsync(int categoryID);

	Task AddProductAsync(Product product);

	Task DeleteProductAsync(Product product);

	Task SaveAsync();

	

}
