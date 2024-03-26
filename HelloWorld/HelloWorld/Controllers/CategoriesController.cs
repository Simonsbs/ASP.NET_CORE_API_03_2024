﻿using AutoMapper;
using HelloWorld.Entities;
using HelloWorld.Models;
using HelloWorld.Repositories;
using HelloWorld.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/categories")]
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

	[HttpGet]
	public async Task<ActionResult<List<CategoryDTO>>> GetCategories(
		string? name,
		string? searchQuery,
		int pageNumber = 1,
		int pageSize = maxPageSize
		) {

		if (pageSize > maxPageSize) {
			pageSize = maxPageSize;
		}

		IEnumerable<Category> categories;
		/*if (string.IsNullOrWhiteSpace(name) && 
			string.IsNullOrWhiteSpace(searchQuery)) {
			categories = await _repo.GetCategoriesAsync();
		} else {*/
			categories = await _repo.GetCategoriesAsync(
				name, 
				searchQuery, 
				pageNumber,
				pageSize);
		//}

		return Ok(_mapper.Map<List<CategoryDTO>>(categories));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCategory(int id, bool includeProducts) {
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
