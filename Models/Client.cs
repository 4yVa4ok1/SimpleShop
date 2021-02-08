using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleShop.Models
{
	public class Client
	{
		[Key]
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime? Birthday { get; set; }

		public DateTime SignUpDate { get; set; }

		public DateTime? LastPurchaseDate { get; set; }

		public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
	}
}
