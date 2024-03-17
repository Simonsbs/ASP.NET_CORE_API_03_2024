using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/categories/{categoryID}/products")]
public class ProductsController : ControllerBase {
    [HttpGet]
    public ActionResult<List<ProductDTO>> GetProducts(int categoryID) {
        if (CategoryExists(categoryID, out CategoryDTO category)) { 
            return NotFound();
        }
        return Ok(category.Products);
    }

    [HttpGet("{productID}")]
    public ActionResult<ProductDTO> GetProduct(int categoryID, int productID) {
        if (CategoryExists(categoryID, out CategoryDTO category)) {
            return NotFound();
        }
        ProductDTO product = category.Products.FirstOrDefault(p => p.ID == productID);
        if (product == null) {
            return NotFound();
        }
        
        return Ok(product);
    }

    private bool CategoryExists(int categoryID, out CategoryDTO category) {
        category = MyDataStore.Categories.FirstOrDefault(c => c.ID == categoryID);
        return category == null;
    }
}