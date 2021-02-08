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
	public class ClientsController : ControllerBase
	{
		readonly ApplicationContext db;

		public ClientsController(ApplicationContext context)
		{
			db = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Client>>> Get()
		{
			return await db.Clients.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Client>> Get(int id)
		{
			Client client = await db.Clients.FirstOrDefaultAsync(c => c.Id == id);
			if (client == null)
				return NotFound();

			return new ObjectResult(client);
		}

		[HttpGet("GetByBirthday")]
		public async Task<ActionResult<IEnumerable<Client>>> GetByBirthday()
		{
			return await GetByBirthday(DateTime.Now);
		}

		[HttpGet("GetByBirthday/{birthday}")]
		public async Task<ActionResult<IEnumerable<Client>>> GetByBirthday(DateTime birthday)
		{
			return await db.Clients.Where(c=>c.Birthday.HasValue && c.Birthday.Value.Date == birthday.Date).Select(c=> new Client{ Id = c.Id, FirstName = c.FirstName, LastName = c.LastName }).ToListAsync();
		}

		[HttpGet("GetLastCustomers")]
		public async Task<ActionResult<IEnumerable<Client>>> GetLastCustomers()
		{
			return await GetLastCustomers(0);
		}

		[HttpGet("GetLastCustomers/{daysCount}")]
		public async Task<ActionResult<IEnumerable<Client>>> GetLastCustomers(int daysCount)
		{
			if(daysCount < 0)
			{
				return BadRequest();
			}
			var filterDate = DateTime.Now.AddDays(-daysCount);
			return await db.Clients.Where(c => c.LastPurchaseDate.HasValue && c.LastPurchaseDate.Value.Date >= filterDate.Date)
				.Select(c => new Client { Id = c.Id, FirstName = c.FirstName, LastName = c.LastName, LastPurchaseDate = c.LastPurchaseDate }).ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult<Client>> Post(Client client)
		{
			if (client == null)
			{
				return BadRequest();
			}

			db.Clients.Add(client);
			await db.SaveChangesAsync();
			return Ok(client);
		}

		[HttpPut]
		public async Task<ActionResult<Client>> Put(Client client)
		{
			if (client == null)
			{
				return BadRequest();
			}
			if (!db.Clients.Any(x => x.Id == client.Id))
			{
				return NotFound();
			}

			db.Clients.Update(client);
			await db.SaveChangesAsync();
			return Ok(client);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Client>> Delete(int id)
		{
			Client client = db.Clients.FirstOrDefault(x => x.Id == id);
			if (client == null)
			{
				return NotFound();
			}
			db.Clients.Remove(client);
			await db.SaveChangesAsync();
			return Ok(client);
		}
	}
}
