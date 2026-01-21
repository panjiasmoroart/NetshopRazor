using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.Pages.Admin.Books;
using System.ComponentModel.DataAnnotations;

namespace NetshopRazor.Pages
{
	[BindProperties]
    public class CartModel : PageModel
    {
		[Required(ErrorMessage = "The Address is required")]
		public string Address { get; set; } = "";

		[Required]
		public string PaymentMethod { get; set; } = "";

		public List<OrderItem> listOrderItems = new List<OrderItem>();
		public decimal subtotal = 0;
		public decimal shippingFee = 5;
		public decimal total = 0;

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
			//Console.WriteLine("Book dictionary count: " + bookDictionary.Count);

			// action can be null, "add", "sub" or "delete"
			string? action = Request.Query["action"];
			string? id = Request.Query["id"];

			if (action != null && id != null && bookDictionary.ContainsKey(id))
			{
				if (action.Equals("add"))
				{
					bookDictionary[id] += 1;
				}
				else if (action.Equals("sub"))
				{
					if (bookDictionary[id] > 1) bookDictionary[id] -= 1;
				}
				else if (action.Equals("delete"))
				{
					bookDictionary.Remove(id);
				}

				// build the new cookie value
				string newCookieValue = "";
				foreach (var keyValuePair in bookDictionary)
				{
					for (int i = 0; i < keyValuePair.Value; i++)
					{
						newCookieValue += "-" + keyValuePair.Key;
					}
				}

				if (newCookieValue.Length > 0)
					newCookieValue = newCookieValue.Substring(1);

				var cookieOptions = new CookieOptions();
				cookieOptions.Expires = DateTime.Now.AddDays(365);
				cookieOptions.Path = "/";

				Response.Cookies.Append("shopping_cart", newCookieValue, cookieOptions);

				// redirect to the same page:
				//   - to remove the query string from the url
				//   - to set the shopping cart size using the updated cookie
				Response.Redirect(Request.Path.ToString());
				return;
			}

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql = "SELECT * FROM books WHERE id=@id";
					foreach (var keyValuePair in bookDictionary)
					{
						string bookID = keyValuePair.Key;
						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("@id", bookID);

							using (SqlDataReader reader = command.ExecuteReader())
							{
								if(reader.Read())
								{
									OrderItem item = new OrderItem();

									item.bookInfo.Id = reader.GetInt32(0);
									item.bookInfo.Title = reader.GetString(1);
									item.bookInfo.Authors = reader.GetString(2);
									item.bookInfo.Isbn = reader.GetString(3);
									item.bookInfo.NumPages = reader.GetInt32(4);
									item.bookInfo.Price = reader.GetDecimal(5);
									item.bookInfo.Category = reader.GetString(6);
									item.bookInfo.Description = reader.GetString(7);
									item.bookInfo.ImageFileName = reader.GetString(8);
									item.bookInfo.CreatedAt = reader.GetDateTime(9).ToString("MM/dd/yyyy");

									item.numCopies = keyValuePair.Value;
									item.totalPrice = item.numCopies * item.bookInfo.Price;

									listOrderItems.Add(item);

									subtotal += item.totalPrice;
									total = subtotal + shippingFee;
								}
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Address = HttpContext.Session.GetString("address") ?? "";
		}
	}


	public class OrderItem
    {
		public BookInfo bookInfo = new BookInfo();
		public int numCopies = 0;
		public decimal totalPrice = 0;
	}
}
