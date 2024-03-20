using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Entities {
	public class Product(string name) {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
