using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleShop.Helpers;

namespace SimpleShop
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(DbHelper.DefaultConnection));
			services.AddControllers().AddNewtonsoftJson(options =>
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
