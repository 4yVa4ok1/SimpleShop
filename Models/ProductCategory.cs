using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleShop.Models
{
	public class ProductCategory
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
