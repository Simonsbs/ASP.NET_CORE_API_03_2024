using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models;

public class ProductForCreationDTO {
    [Required]
    [MaxLength(100)]
    public string Name {
        get; set;
    }

    [MaxLength(500)]
    public string Description {
        get; set;
    }

    [Required]
    [Range(1, double.MaxValue)]
    public double Price {
        get; set;
    }
}
