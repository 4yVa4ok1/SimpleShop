using Microsoft.EntityFrameworkCore;
using SimpleShop.Helpers;
using SimpleShop.Models;
using System;
using System.Collections.Generic;

namespace SimpleShop
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Client> Clients { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Purchase> Purchases { get; set; }

		public DbSet<ProductCategory> ProductCategories { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
			try
			{
				Database.EnsureCreated();
			}
			catch(Exception)
			{
			}
			
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(DbHelper.DefaultConnection);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Product>()
				.HasMany(c => c.Purchases)
				.WithMany(s => s.Products)
				.UsingEntity<PurchaseProduct>(j => j
					.HasOne(pt => pt.Purchase)
					.WithMany(t => t.PurchaseProducts)
					.HasForeignKey(pt => pt.PurchaseId),
				j => j
					.HasOne(pt => pt.Product)
					.WithMany(p => p.PurchaseProducts)
					.HasForeignKey(pt => pt.ProductId),
				j =>
				{
					j.HasKey(t => new { t.ProductId, t.PurchaseId });
					j.ToTable("PurchaseProducts");
				}
			);

			var now = DateTime.Now;
			modelBuilder.Entity<Client>().HasData(
				new List<Client>
				{
					new Client() { Id = 1, FirstName = "Timur", LastName = "Hiptenko", Birthday = new DateTime(1997, 07, 23), SignUpDate = now, LastPurchaseDate = now },
					new Client() { Id = 2, FirstName = "Evgeniy", LastName = "Yakush", Birthday = new DateTime(1995, 08, 01), SignUpDate = now, LastPurchaseDate = DateTime.Now.AddDays(-1) },
					new Client() { Id = 3, FirstName = "Yaroslav", LastName = "Shelest", Birthday = now, SignUpDate = now, LastPurchaseDate = DateTime.Now.AddDays(-2) }
				});

			modelBuilder.Entity<ProductCategory>().HasData(
				new List<ProductCategory>
				{
					new ProductCategory() { Id = 1, Name = "Продукты питания" },
					new ProductCategory() { Id = 2, Name = "Средства личной гигиены" },
					new ProductCategory() { Id = 3, Name = "Специи" }
				});

			modelBuilder.Entity<Product>().HasData(
				new List<Product>
				{
					new Product() { Id = 1, Name = "Хлеб", ProductCategoryId = 1, VendorCode = "ХЛ-01", Price = 5 },
					new Product() { Id = 2, Name = "Вода", ProductCategoryId = 1, VendorCode = "ВО-01", Price = 3 },
					new Product() { Id = 3, Name = "Мыло", ProductCategoryId = 2, VendorCode = "МЛ-01", Price = 7.2M },
					new Product() { Id = 4, Name = "Перец", ProductCategoryId = 3, VendorCode = "ПЕ-01", Price = 2.5M }
				});
		}
	}
}
