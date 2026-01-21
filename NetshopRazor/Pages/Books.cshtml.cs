using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.Pages.Admin.Books;

namespace NetshopRazor.Pages
{
	[BindProperties(SupportsGet = true)]
	public class BooksModel : PageModel
    {
		public string? Search { get; set; }
		 //<option value = "any" > Any </ option >
		public string PriceRange { get; set; } = "any";
		public string PageRange { get; set; } = "any";
		public string Category { get; set; } = "any";

		public List<BookInfo> listBooks = new List<BookInfo>();

		private readonly string connectionString;
		public BooksModel(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection")!;
		}

		public void OnGet()
        {
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql = "SELECT * FROM books";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								BookInfo bookInfo = new BookInfo();

								bookInfo.Id = reader.GetInt32(0);
								bookInfo.Title = reader.GetString(1);
								bookInfo.Authors = reader.GetString(2);
								bookInfo.Isbn = reader.GetString(3);
								bookInfo.NumPages = reader.GetInt32(4);
								bookInfo.Price = reader.GetDecimal(5);
								bookInfo.Category = reader.GetString(6);
								bookInfo.Description = reader.GetString(7);
								bookInfo.ImageFileName = reader.GetString(8);
								bookInfo.CreatedAt = reader.GetDateTime(9).ToString("MM/dd/yyyy");

								listBooks.Add(bookInfo);
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
}
