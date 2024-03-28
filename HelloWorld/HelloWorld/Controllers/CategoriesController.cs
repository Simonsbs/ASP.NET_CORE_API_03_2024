using System.Xml.Linq;
using Asp.Versioning;
using AutoMapper;
using HelloWorld.Entities;
using HelloWorld.Models;
using HelloWorld.Repositories;
using HelloWorld.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;


[ApiController]
[Route("api/v{version:ApiVersion}/categories")]
[ApiVersion(1)]
[ApiVersion(2)]
[Authorize]
public class CategoriesController : ControllerBase {
	private ILogger<CategoriesController> _logger;
	private readonly ICategoryRepository _repo;
	private readonly IMapper _mapper;
	private const int maxPageSize = 10;


	public CategoriesController(
		ILogger<CategoriesController> logger,
		ICategoryRepository categoryRepository,
		IMapper mapper
		) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_repo = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	/// <summary>
	/// Get all the categories
	/// </summary>
	/// <param name="name">filter the results by name of category</param>
	/// <param name="searchQuery">seaarch the results for a phrase</param>
	/// <param name="pageNumber">the page number top return (default 1)</param>
	/// <param name="pageSize">the size of the page to return (default 10)</param>
	/// <returns>the resulting list of categories by page</returns>
	[HttpGet]
	//[Authorize(Policy = "IsAdmin")]
	public async Task<ActionResult<List<CategoryDTO>>> GetCategories(
		string? name,
		string? searchQuery,
		int pageNumber = 1,
		int pageSize = maxPageSize
		) {


		if (pageSize > maxPageSize) {
			pageSize = maxPageSize;
		}

		/*IEnumerable<Category> categories;
		if (string.IsNullOrWhiteSpace(name) && 
			string.IsNullOrWhiteSpace(searchQuery)) {
			categories = await _repo.GetCategoriesAsync();
		} else {
			categories = await _repo.GetCategoriesAsync(
				name, 
				searchQuery, 
				pageNumber,
				pageSize);
		}*/

		var (categories, meta) = await _repo.GetCategoriesAsync(
				name,
				searchQuery,
				pageNumber,
				pageSize);

		Response.Headers.Add("X-PageNumber", meta.PageNumber.ToString());
		Response.Headers.Add("X-PageSize", meta.PageSize.ToString());
		Response.Headers.Add("X-TotalItemCount", meta.TotalItemCount.ToString());

		return Ok(_mapper.Map<List<CategoryDTO>>(categories));
	}

	/// <summary>
	/// Get a single category by ID
	/// </summary>
	/// <param name="id">the id of the catgegory to return</param>
	/// <param name="includeProducts">a flag that indicates if we want the products returned in category object</param>
	/// <returns>A category</returns>
	/// <response code="200">A valid category with the given ID</response>
	/// <response code="403">User is forbiden, does not have allowed_category claim</response>
	/// <response code="404">No category with the given id was found</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetCategory(int id, bool includeProducts) {

		if (User.Claims.
			FirstOrDefault(c => c.Type == "allowed_category")?.
			Value != id.ToString()) {

			return Forbid();
		}

		Category? category = await _repo.GetCategoryAsync(id, includeProducts);
		if (category == null) {
			return NotFound();
		}

		if (includeProducts) {
			return Ok(_mapper.Map<CategoryDTO>(category));
		}

		return Ok(_mapper.Map<CategoryWithNoProductsDTO>(category));


		//CategoryDTO category = MyDataStore.Categories.FirstOrDefault(c => c.ID == id);
		//      if (category == null) {
		//          return NotFound();
		//      }
		//      return Ok(category);
	}
}
