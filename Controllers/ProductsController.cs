using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		readonly ApplicationContext db;

		public ProductsController(ApplicationContext context)
		{
			db = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> Get()
		{
			return await db.Products.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> Get(int id)
		{
			Product product = await db.Products.FirstOrDefaultAsync(c => c.Id == id);
			if (product == null)
				return NotFound();

			return new ObjectResult(product);
		}

		[HttpGet("GetByPurchase/{purchaseId}")]
		public async Task<ActionResult<IEnumerable<Product>>> GetByPurchase(int purchaseId)
		{
			Purchase purchase = await db.Purchases.Include(p => p.Products).ThenInclude(pr => pr.ProductCategory)
				.FirstOrDefaultAsync(p => p.Id == purchaseId);
			if (purchase == null)
				return NotFound();

			return new ObjectResult(purchase.Products.Select(p => new
			{
				p.Name, Category = p.ProductCategory.Name, p.VendorCode, p.Price,
				p.PurchaseProducts.FirstOrDefault(pp => pp.PurchaseId == purchaseId).Count
			}));
		}

		[HttpPost]
		public async Task<ActionResult<Product>> Post(Product product)
		{
			if (product == null)
			{
				return BadRequest();
			}

			db.Products.Add(product);
			await db.SaveChangesAsync();
			return Ok(product);
		}

		[HttpPut]
		public async Task<ActionResult<Product>> Put(Product product)
		{
			if (product == null)
			{
				return BadRequest();
			}
			if (!db.Products.Any(x => x.Id == product.Id))
			{
				return NotFound();
			}

			db.Products.Update(product);
			await db.SaveChangesAsync();
			return Ok(product);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Product>> Delete(int id)
		{
			Product product = db.Products.FirstOrDefault(x => x.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			db.Products.Remove(product);
			await db.SaveChangesAsync();
			return Ok(product);
		}
	}
}
