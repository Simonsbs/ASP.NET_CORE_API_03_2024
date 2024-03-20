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
    public async Task<ActionResult<List<CategoryDTO>>> GetCategories() {
		IEnumerable<Category> categories = await _repo.GetCategoriesAsync();

        return Ok(_mapper.Map<List<CategoryDTO>>(categories));
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
