using System.Data.SqlClient;
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
			//Console.WriteLine("Book dictionary count: " + bookDictionary.Count);

			// action can be null, "add", "sub" or "delete"
			string? action = Request.Query["action"];
			string? id = Request.Query["id"];

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
		}
	}


	public class OrderItem
    {
		public BookInfo bookInfo = new BookInfo();
		public int numCopies = 0;
		public decimal totalPrice = 0;
	}
}
