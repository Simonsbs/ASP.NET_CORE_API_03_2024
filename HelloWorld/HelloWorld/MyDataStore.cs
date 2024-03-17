using HelloWorld.Models;

namespace HelloWorld {
    public static class MyDataStore {
        public static List<CategoryDTO> Categories {
            get; set;
        } = new List<CategoryDTO>() {
            new CategoryDTO { ID = 1, Name = "Books" ,
                Products = new List<ProductDTO> {
                    new ProductDTO {
                        ID = 1,
                        Name = "Product 1",
                        Description = "A product",
                        Price = 100.0
                    },
                    new ProductDTO {
                        ID = 2,
                        Name = "Product 2",
                        Description = "A product",
                        Price = 100.0
                    },
                    new ProductDTO {
                        ID = 3,
                        Name = "Product 3",
                        Description = "A product",
                        Price = 100.0
                    }
                }
            },
            new CategoryDTO { ID = 2, Name = "Shoes" ,
                Products = new List<ProductDTO> {
                    new ProductDTO {
                        ID = 4,
                        Name = "Product 4",
                        Description = "A product",
                        Price = 100.0
                    },
                    new ProductDTO {
                        ID = 5,
                        Name = "Product 5",
                        Description = "A product",
                        Price = 100.0
                    },
                    new ProductDTO {
                        ID = 6,
                        Name = "Product 6",
                        Description = "A product",
                        Price = 100.0
                    }
                }}
        };
    }
}
