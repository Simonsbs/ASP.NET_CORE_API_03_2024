using HelloWorld.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Contexts;

public class MainContext : DbContext {
	public MainContext(DbContextOptions<MainContext> options) : base(options) {

	}

	DbSet<Category> Categories { get; set; }
	DbSet<Product> Products { get; set; }

	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
	//	optionsBuilder.UseSqlite("connectionstring");

	//	base.OnConfiguring(optionsBuilder);
	//}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {

		//modelBuilder.Entity<Category>().HasData(
		//	new Category("Category 1") {
		//		ID = 1
		//	},
		//	new Category("Category 2") {
		//		ID = 2
		//	},
		//	new Category("Category 3") {
		//		ID = 3
		//	}
		//	);

		//modelBuilder.Entity<Product>().HasData(
		//	new Product("Product 1") {
		//		ID = 1,
		//		Description = "Description 1",
		//		CategoryID = 1
		//	},
		//	new Product("Product 2") {
		//		ID = 2,
		//		CategoryID = 1
		//	},
		//	new Product("Product 3") {
		//		ID = 3,
		//		Description = "Description 3",
		//		CategoryID = 1
		//	},
		//	new Product("Product 4") {
		//		ID = 4,
		//		Description = "Description 4",
		//		CategoryID = 2
		//	},
		//	new Product("Product 5") {
		//		ID = 5,
		//		Description = "Description 5",
		//		CategoryID = 2
		//	},
		//	new Product("Product 6") {
		//		ID = 6,
		//		CategoryID = 2
		//	},
		//	new Product("Product 7") {
		//		ID = 7,
		//		Description = "Description 7",
		//		CategoryID = 3
		//	},
		//	new Product("Product 8") {
		//		ID = 8,
		//		CategoryID = 3
		//	},
		//	new Product("Product 9") {
		//		ID = 9,
		//		Description = "Description 9",
		//		CategoryID = 3
		//	}
		//	);

		base.OnModelCreating(modelBuilder);
	}
}
