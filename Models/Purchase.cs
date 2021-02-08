using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleShop.Models
{
	public class Purchase
	{
		[Key]
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public decimal Sum { get; set; }


		public int? ClientId { get; set; }

		public virtual Client Client { get; set; }



		public ICollection<Product> Products { get; set; } = new List<Product>();

		public ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();
	}
}
