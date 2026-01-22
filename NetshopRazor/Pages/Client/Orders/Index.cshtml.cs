using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.MyHelpers;
using NetshopRazor.Pages.Admin.Orders;


namespace NetshopRazor.Pages.Client.Orders
{
	[RequireAuth(RequiredRole = "client")]
	public class IndexModel : PageModel
    {
		public List<OrderInfo> listOrders = new List<OrderInfo>();

		public int page = 1; // the current html page
		public int totalPages = 0;
		private readonly int pageSize = 3; // orders per page

		private readonly string connectionString;
		public IndexModel(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection")!;
		}

		public void OnGet()
        {
			
		}
    }
}
