using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.Pages.Admin.Books;

namespace NetshopRazor.Pages
{
    public class CartModel : PageModel
    {
		public List<OrderItem> listOrderItems = new List<OrderItem>();

		private readonly string connectionString;
		public CartModel(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection")!;
		}

		private Dictionary<String, int> getBookDictionary()
		{
			var bookDictionary = new Dictionary<string, int>();

			// Read shopping cart items from cookie
			string cookieValue = Request.Cookies["shopping_cart"] ?? "";

			if (cookieValue.Length > 0)
			{
				string[] bookIdArray = cookieValue.Split('-');

				for (int i = 0; i < bookIdArray.Length; i++)
				{
					string bookId = bookIdArray[i];
					if (bookDictionary.ContainsKey(bookId))
					{
						bookDictionary[bookId] += 1;
					}
					else
					{
						bookDictionary.Add(bookId, 1);
					}
				}
			}

			return bookDictionary;
		}

		public void OnGet()
        {
			// Read shopping cart items from cookie
			var bookDictionary = getBookDictionary();
			Console.WriteLine("Book dictionary count: " + bookDictionary.Count);
		}
	}


	public class OrderItem
    {
		public BookInfo bookInfo = new BookInfo();
		public int numCopies = 0;
		public decimal totalPrice = 0;
	}
}
