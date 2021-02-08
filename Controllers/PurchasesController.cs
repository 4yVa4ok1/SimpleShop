using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PurchasesController : ControllerBase
	{
		readonly ApplicationContext db;

		public PurchasesController(ApplicationContext context)
		{
			db = context;
			if (!db.Purchases.Any())
			{
				GeneratePurchases();
			}
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Purchase>>> Get()
		{
			return await db.Purchases.Select(p=> new Purchase{ Id = p.Id, ClientId = p.ClientId, Client = p.Client, Date = p.Date, Sum = p.Sum }).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Purchase>> Get(int id)
		{
			Purchase purchase = await db.Purchases.FirstOrDefaultAsync(c => c.Id == id);
			if (purchase == null)
				return NotFound();

			return new ObjectResult(purchase);
		}

		[HttpPost]
		public async Task<ActionResult<Purchase>> Post(Purchase purchase)
		{
			if (purchase == null)
			{
				return BadRequest();
			}

			db.Purchases.Add(purchase);
			await db.SaveChangesAsync();
			return Ok(purchase);
		}

		[HttpPut]
		public async Task<ActionResult<Purchase>> Put(Purchase purchase)
		{
			if (purchase == null)
			{
				return BadRequest();
			}
			if (!db.Purchases.Any(x => x.Id == purchase.Id))
			{
				return NotFound();
			}

			db.Purchases.Update(purchase);
			await db.SaveChangesAsync();
			return Ok(purchase);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Purchase>> Delete(int id)
		{
			Purchase purchase = db.Purchases.FirstOrDefault(x => x.Id == id);
			if (purchase == null)
			{
				return NotFound();
			}
			db.Purchases.Remove(purchase);
			await db.SaveChangesAsync();
			return Ok(purchase);
		}

		private void GeneratePurchases()
		{
			var random = new Random();
			for (int i = 1; i < 6; i++)
			{
				var clientId = random.Next(1, 4);
				var purchase = new Purchase() { Date = db.Clients.First(c=>c.Id == clientId).LastPurchaseDate ?? new DateTime(2020, 12, 1), ClientId = clientId };
				for (int n = 1; n < random.Next(4, 12); n++)
				{
					var productId = random.Next(1, 5);
					var product = db.Products.First(p => p.Id == productId);
					if (purchase.PurchaseProducts.FirstOrDefault(p=>p.Product == product) != null)
					{
						purchase.PurchaseProducts.FirstOrDefault(p => p.Product == product).Count += random.Next(2, 16);
					}
					else
					{
						purchase.PurchaseProducts.Add(new PurchaseProduct() { Product = product, Count = random.Next(2, 16) });
					}
					purchase.Sum = purchase.PurchaseProducts.Sum(pp => pp.Product.Price * pp.Count);
					db.Purchases.Add(purchase);
				}
			}
			db.SaveChanges();
		}
	}
}
