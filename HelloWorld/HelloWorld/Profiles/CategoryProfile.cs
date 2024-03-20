using AutoMapper;
using HelloWorld.Entities;
using HelloWorld.Models;

namespace HelloWorld.Profiles;

public class CategoryProfile : Profile {
	public CategoryProfile() {
		CreateMap<Category, CategoryDTO>();
		CreateMap<Category, CategoryWithNoProductsDTO>();
	}
}
