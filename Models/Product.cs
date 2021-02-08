using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleShop.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public string VendorCode { get; set; }

		public decimal Price { get; set; }



		public int? ProductCategoryId { get; set; }

		public virtual ProductCategory ProductCategory { get; set; }



		public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

		public ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();
	}
}
