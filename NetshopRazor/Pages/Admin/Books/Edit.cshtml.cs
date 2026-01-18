using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetshopRazor.Pages.Admin.Books
{
    public class EditModel : PageModel
    {
		[BindProperty]
		public int Id { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "The Title is required")]
		[MaxLength(100, ErrorMessage = "The Title cannot exceed 100 characters")]
		public string Title { get; set; } = "";

		[BindProperty]
		[Required(ErrorMessage = "The Author is required")]
		[MaxLength(255, ErrorMessage = "The Authors cannot exceed 255 characters")]
		public string Authors { get; set; } = "";

		[BindProperty]
		[Required(ErrorMessage = "The ISBN is required")]
		[MaxLength(20, ErrorMessage = "The ISBN cannot exceed 20 characters")]
		public string ISBN { get; set; } = "";

		[BindProperty]
		[Required(ErrorMessage = "The Number of Pages is required")]
		[Range(1, 10000, ErrorMessage = "The Number of Pages must be in the range from 1 to 10000")]
		public int NumPages { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "The Price is required")]
		public decimal Price { get; set; }

		[BindProperty, Required]
		public string Category { get; set; } = "";

		[BindProperty]
		[MaxLength(1000, ErrorMessage = "The Description cannot exceed 1000 characters")]
		public string? Description { get; set; } = "";

		[BindProperty]
		public string ImageFileName { get; set; } = "";

		[BindProperty]
		public IFormFile? ImageFile { get; set; }

		public string errorMessage = "";
		public string successMessage = "";

		private IWebHostEnvironment webHostEnvironment;

		public EditModel(IWebHostEnvironment env)
		{
			webHostEnvironment = env;
		}

		public void OnGet()
        {
			string requestId = Request.Query["id"];

			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=netshoprazor_db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql = "SELECT * FROM books WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", requestId);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								Id = reader.GetInt32(0);
								Title = reader.GetString(1);
								Authors = reader.GetString(2);
								ISBN = reader.GetString(3);
								NumPages = reader.GetInt32(4);
								Price = reader.GetDecimal(5);
								Category = reader.GetString(6);
								Description = reader.GetString(7);
								ImageFileName = reader.GetString(8);
							}
							else
							{
								Response.Redirect("/Admin/Books/Index");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Response.Redirect("/Admin/Books/Index");
			}
		}

		public void OnPost()
		{
			if (!ModelState.IsValid)
			{
				errorMessage = "Data validation failed";
				return;
			}

			// successfull data validation

			if (Description == null) Description = "";

			// if we have a new ImageFile => upload the new image and delete the old image

			// update the book data in the database

			successMessage = "Data saved correctly";
			//Response.Redirect("/Admin/Books/Index");
		}
    }
}
