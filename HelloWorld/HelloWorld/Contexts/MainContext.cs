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
}
