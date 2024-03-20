using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Entities {
	public class Category(string name) {
		public int ID {
			get; set;
		}

		[Required]
		[MaxLength(100)]
		public string Name {
            get; set;
        } = name;

	}
}
