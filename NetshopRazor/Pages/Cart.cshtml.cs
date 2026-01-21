using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.Pages.Admin.Books;

namespace NetshopRazor.Pages
{
    public class CartModel : PageModel
    {
		public List<OrderItem> listOrderItems = new List<OrderItem>();
		public void OnGet()
        {
        }
    }

    public class OrderItem
    {
		public BookInfo bookInfo = new BookInfo();
		public int numCopies = 0;
		public decimal totalPrice = 0;
	}
}
