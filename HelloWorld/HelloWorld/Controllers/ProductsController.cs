﻿using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/categories/{categoryID}/products")]
public class ProductsController : ControllerBase {
    private ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger) {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public ActionResult<List<ProductDTO>> GetProducts(int categoryID) {
        if (CategoryNotExists(categoryID, out CategoryDTO category)) {
            _logger.LogWarning($"Some one was lookg for category with id: {categoryID}");
            return NotFound();
        }
        return Ok(category.Products);
    }

    [HttpGet("{productID}", Name = "GetProduct")]
    public ActionResult<ProductDTO> GetProduct(int categoryID, int productID) {
        if (CategoryNotExists(categoryID, out CategoryDTO category)) {
            _logger.LogWarning($"Some one was lookg for category with id: {categoryID}");
            return NotFound();
        }
        ProductDTO product = category.Products.FirstOrDefault(p => p.ID == productID);
        if (product == null) {
            _logger.LogWarning($"Some one was lookg for product with id: {productID}");
            return NotFound();
        }


        _logger.LogCritical($"LOOK AT ME!!!");
        return Ok(product);
    }

    private bool CategoryNotExists(int categoryID, out CategoryDTO category) {
        category = MyDataStore.Categories.FirstOrDefault(c => c.ID == categoryID);
        return category == null;
    }

    [HttpPost]
    public ActionResult CreateProduct(int categoryID, ProductForCreationDTO product) {
        if (CategoryNotExists(categoryID, out CategoryDTO category)) {
            return NotFound();
        }

        int maxID = MyDataStore.Categories.
                                SelectMany(c => c.Products).
                                Max(p => p.ID);

        ProductDTO newProduct = new ProductDTO {
            ID = ++maxID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        };

        category.Products.Add(newProduct);

        return CreatedAtRoute("GetProduct", new {
            categoryID,
            productID = newProduct.ID
        }, newProduct);
    }

    [HttpPut("{productID}")]
    public ActionResult UpdateProduct(int categoryID, int productID, ProductForUpdateDTO product) {
        if (CategoryNotExists(categoryID, out CategoryDTO category)) {
            return NotFound();
        }

        ProductDTO productFromStore = category.Products.FirstOrDefault(p => p.ID == productID);
        if (productFromStore == null) {
            return NotFound();
        }

        productFromStore.Name = product.Name;
        productFromStore.Description = product.Description;
        productFromStore.Price = product.Price;

        return NoContent();
    }

    [HttpDelete("{productID}")]
    public ActionResult DeleteProduct(int categoryID, int productID) {
        if (CategoryNotExists(categoryID, out CategoryDTO category)) {
            return NotFound();
        }

        ProductDTO productFromStore = category.Products.FirstOrDefault(p => p.ID == productID);
        if (productFromStore == null) {
            return NotFound();
        }

        category.Products.Remove(productFromStore);

        return NoContent();
    }
}//Simon