using AutoMapper;
using HelloWorld.Entities;
using HelloWorld.Models;
using HelloWorld.Repositories;
using HelloWorld.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/categories/{categoryID}/products")]
[Authorize]
public class ProductsController : ControllerBase {
	private ILogger<ProductsController> _logger;
	private IMailService _mailService;
	private readonly IProductRepository _repo;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IMapper _mapper;

	public ProductsController(
		ILogger<ProductsController> logger,
		IMailService mailService,
		IProductRepository productRepository,
		ICategoryRepository categoryRepository,
		IMapper mapper
		) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
		_repo = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
		_categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<ProductDTO>>> GetAllProducts() {
		IEnumerable<Product> products = await _repo.GetProductsAsync();

		//List<ProductDTO> result = new List<ProductDTO>();
		//foreach (Product product in products) {
		//    result.Add(new ProductDTO {
		//        ID = product.ID,
		//        Name = product.Name,
		//        Description = product.Description,
		//        Price = 0
		//    });
		//}

		List<ProductDTO> result = _mapper.Map<List<ProductDTO>>(products);

		return Ok(result);
	}

	[HttpGet]
	public async Task<ActionResult<List<ProductDTO>>> GetProducts(int categoryID) {
		if (!await _categoryRepository.CategoryExistsAsync(categoryID)) {
			_logger.LogWarning("Category not found");
			return NotFound("Category not found");
		}

		IEnumerable<Product> products = await _repo.
			GetProductsForCategoryAsync(categoryID);

		return Ok(_mapper.Map<List<ProductDTO>>(products));

		//if (CategoryNotExists(categoryID, out CategoryDTO category)) {
		//	_logger.LogWarning($"Some one was lookg for category with id: {categoryID}");
		//	return NotFound();
		//}
		//return Ok(category.Products);
	}

	[HttpGet("{productID}", Name = "GetProduct")]
	public async Task<ActionResult<ProductDTO>> GetProduct(int categoryID, int productID) {
		if (!await _categoryRepository.CategoryExistsAsync(categoryID)) {
			_logger.LogWarning("Category not found");
			return NotFound("Category not found");
		}

		Product? product = await _repo.GetProductForCategoryAsync(categoryID, productID);

		if (product == null) {
			return NotFound();
		}

		return Ok(_mapper.Map<ProductDTO>(product));

		//if (CategoryNotExists(categoryID, out CategoryDTO category)) {
		//	_logger.LogWarning($"Some one was lookg for category with id: {categoryID}");
		//	return NotFound();
		//}
		//ProductDTO product = category.Products.FirstOrDefault(p => p.ID == productID);
		//if (product == null) {
		//	_logger.LogWarning($"Some one was lookg for product with id: {productID}");
		//	return NotFound();
		//}


		//_logger.LogCritical($"LOOK AT ME!!!");
		//return Ok(product);
	}

	private bool CategoryNotExists(int categoryID, out CategoryDTO category) {
		category = MyDataStore.Categories.FirstOrDefault(c => c.ID == categoryID);
		return category == null;
	}

	[HttpPost]
	public async Task<ActionResult> CreateProduct(int categoryID, 
		ProductForCreationDTO product) {

		if (!await _categoryRepository.CategoryExistsAsync(categoryID)) {
			_logger.LogWarning("Category not found");
			return NotFound("Category not found");
		}

		Product productToCreate = _mapper.Map<Product>(product);
		productToCreate.CategoryID = categoryID;

		await _repo.AddProductAsync(productToCreate);

		// await _repo.SaveAsync();

		return CreatedAtRoute("GetProduct", new {
			categoryID,
			productID = productToCreate.ID
		}, productToCreate);

		//if (CategoryNotExists(categoryID, out CategoryDTO category)) {
		//	return NotFound();
		//}

		//int maxID = MyDataStore.Categories.
		//						SelectMany(c => c.Products).
		//						Max(p => p.ID);

		//ProductDTO newProduct = new ProductDTO {
		//	ID = ++maxID,
		//	Name = product.Name,
		//	Description = product.Description,
		//	Price = product.Price,
		//};

		//category.Products.Add(newProduct);

		//return CreatedAtRoute("GetProduct", new {
		//	categoryID,
		//	productID = newProduct.ID
		//}, newProduct);
	}

	[HttpPut("{productID}")]
	public async Task<ActionResult> UpdateProduct(int categoryID, 
		int productID, 
		ProductForUpdateDTO product) {

		if (!await _categoryRepository.CategoryExistsAsync(categoryID)) {
			_logger.LogWarning("Category not found");
			return NotFound("Category not found");
		}

		Product? productToUpdate = await _repo.
					GetProductForCategoryAsync(categoryID, productID);

		if (productToUpdate == null) {
			return NotFound();
		}

		_mapper.Map(product, productToUpdate);

		await _repo.SaveAsync();

		return NoContent();


		//if (CategoryNotExists(categoryID, out CategoryDTO category)) {
		//	return NotFound();
		//}

		//ProductDTO productFromStore = category.Products.FirstOrDefault(p => p.ID == productID);
		//if (productFromStore == null) {
		//	return NotFound();
		//}

		//productFromStore.Name = product.Name;
		//productFromStore.Description = product.Description;
		//productFromStore.Price = product.Price;

		//return NoContent();
	}

	[HttpDelete("{productID}")]
	public async Task<ActionResult> DeleteProduct(
		int categoryID, 
		int productID) {

		if (!await _categoryRepository.CategoryExistsAsync(categoryID)) {
			_logger.LogWarning("Category not found");
			return NotFound("Category not found");
		}

		Product? productToDelete = await _repo.
					GetProductForCategoryAsync(categoryID, productID);

		if (productToDelete == null) {
			return NotFound();
		}

		await _repo.DeleteProductAsync(productToDelete);

		return NoContent();

		//if (CategoryNotExists(categoryID, out CategoryDTO category)) {
		//	return NotFound();
		//}

		//ProductDTO productFromStore = category.Products.FirstOrDefault(p => p.ID == productID);
		//if (productFromStore == null) {
		//	return NotFound();
		//}

		//category.Products.Remove(productFromStore);

		//_mailService.Send("Product deleted", $"a user deleted the product {productFromStore.Name}");

		//return NoContent();
	}
}