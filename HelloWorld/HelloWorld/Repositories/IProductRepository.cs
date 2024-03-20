using HelloWorld.Entities;

namespace HelloWorld.Repositories;

public interface IProductRepository {
	Task<IEnumerable<Product>> GetProductsAsync();

}
