using AutoMapper;
using HelloWorld.Entities;
using HelloWorld.Models;

namespace HelloWorld.Profiles;

public class ProductProfile : Profile {
	public ProductProfile() {
		CreateMap<Product, ProductDTO>()
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ID * 2));
		CreateMap<ProductForCreationDTO, Product>();
	}
}
