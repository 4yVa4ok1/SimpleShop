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
	public class ProductCategoriesController : ControllerBase
	{
		readonly ApplicationContext db;

		public ProductCategoriesController(ApplicationContext context)
		{
			db = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductCategory>>> Get()
		{
			return await db.ProductCategories.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductCategory>> Get(int id)
		{
			ProductCategory productCategory = await db.ProductCategories.FirstOrDefaultAsync(c => c.Id == id);
			if (productCategory == null)
				return NotFound();

			return new ObjectResult(productCategory);
		}

		[HttpGet("GetByClient/{clientId}")]
		public async Task<ActionResult<ProductCategory>> GetByClient(int clientId)
		{
			Client client = await db.Clients.Include(c=>c.Purchases).ThenInclude(pu=>pu.Products)
				.ThenInclude(pr=>pr.ProductCategory).FirstOrDefaultAsync(c => c.Id == clientId);
			if (client == null)
				return NotFound();

			return new ObjectResult(client.Purchases.SelectMany(pu=> pu.PurchaseProducts,
				(pu, pr)=> new { Name = pr.Product.Name, VendorCode = pr.Product.VendorCode, Count = pr.Count }).GroupBy(e=> new { e.Name, e.VendorCode })
				.Select(g=> new { Name = g.Key.Name + " " + g.Key.VendorCode, Count = g.Sum(gr=>gr.Count) }).ToList());
		}

		[HttpPost]
		public async Task<ActionResult<ProductCategory>> Post(ProductCategory productCategory)
		{
			if (productCategory == null)
			{
				return BadRequest();
			}

			db.ProductCategories.Add(productCategory);
			await db.SaveChangesAsync();
			return Ok(productCategory);
		}

		[HttpPut]
		public async Task<ActionResult<ProductCategory>> Put(ProductCategory productCategory)
		{
			if (productCategory == null)
			{
				return BadRequest();
			}
			if (!db.ProductCategories.Any(x => x.Id == productCategory.Id))
			{
				return NotFound();
			}

			db.ProductCategories.Update(productCategory);
			await db.SaveChangesAsync();
			return Ok(productCategory);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ProductCategory>> Delete(int id)
		{
			ProductCategory productCategory = db.ProductCategories.Include("Products").FirstOrDefault(x => x.Id == id);
			if (productCategory == null)
			{
				return NotFound();
			}
			db.ProductCategories.Remove(productCategory);
			await db.SaveChangesAsync();
			return Ok(productCategory);
		}
	}
}
