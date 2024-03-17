using HelloWorld.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase {
    [HttpGet]
    public ActionResult<List<CategoryDTO>> GetCategories() {
        return Ok(MyDataStore.Categories);
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryDTO> GetCategory(int id) {
        CategoryDTO category = MyDataStore.Categories.FirstOrDefault(c => c.ID == id);
        if (category == null) {
            return NotFound();
        }
        return Ok(category);
    }
}
