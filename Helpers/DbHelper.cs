namespace SimpleShop.Helpers
{
	public static class DbHelper
	{
		const string defaultConnetion = "Data Source=4YVA4OK;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		public static string DefaultConnection { get { return defaultConnetion; } }
	}
}
