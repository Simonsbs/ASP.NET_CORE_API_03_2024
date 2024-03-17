namespace HelloWorld.Models;

public class CategoryDTO {
    public int ID {
        get; set;
    }

    public string Name {
        get; set;
    }

    public int ProductCount {
        get; set;
    }

    public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
}
