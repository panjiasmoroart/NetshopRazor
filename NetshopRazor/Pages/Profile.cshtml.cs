using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.MyHelpers;

namespace NetshopRazor.Pages
{
	[RequireAuth]
	[BindProperties]
	public class ProfileModel : PageModel
    {
		[Required(ErrorMessage = "The First Name is required")]
		public string Firstname { get; set; } = "";

		[Required(ErrorMessage = "The Last Name is required")]
		public string Lastname { get; set; } = "";

		[Required(ErrorMessage = "The Email is required"), EmailAddress]
		public string Email { get; set; } = "";

		public string? Phone { get; set; } = "";

		[Required(ErrorMessage = "The Address is required")]
		public string Address { get; set; } = "";

		public string? Password { get; set; } = "";
		public string? ConfirmPassword { get; set; } = "";

		public string errorMessage = "";
		public string successMessage = "";

		public void OnGet()
        {
			Firstname = HttpContext.Session.GetString("firstname") ?? "";
			Lastname = HttpContext.Session.GetString("lastname") ?? "";
			Email = HttpContext.Session.GetString("email") ?? "";
			Phone = HttpContext.Session.GetString("phone") ?? "";
			Address = HttpContext.Session.GetString("address") ?? "";
		}

		public void OnPostProfileOld()
		{
			if (!ModelState.IsValid)
			{
				errorMessage = "Data validation failed";
				return;
			}

			

			string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=netshoprazor_db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

			// successful data validation
			//if (Phone == null) Phone = "";

			// update the user profile or the password
			string submitButton = Request.Form["action"];

			if (submitButton.Equals("profile"))
			{
				Console.WriteLine("action : >>>>>> " + "profile");
				// update the user profile in the database
				try
				{
					using (SqlConnection connection = new SqlConnection(connectionString))
					{
						connection.Open();

						string sql = "UPDATE users SET firstname=@firstname, lastname=@lastname, " +
							"email=@email, phone=@phone, address=@address WHERE id=@id";

						int? id = HttpContext.Session.GetInt32("id");
						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("@firstname", Firstname);
							command.Parameters.AddWithValue("@lastname", Lastname);
							command.Parameters.AddWithValue("@email", Email);
							command.Parameters.AddWithValue("@phone", Phone);
							command.Parameters.AddWithValue("@address", Address);
							command.Parameters.AddWithValue("@id", id);

							command.ExecuteNonQuery();
						}
					}
				}
				catch (Exception ex)
				{
					errorMessage = ex.Message;
					return;
				}

				// update the session data
				HttpContext.Session.SetString("firstname", Firstname);
				HttpContext.Session.SetString("lastname", Lastname);
				HttpContext.Session.SetString("email", Email);
				HttpContext.Session.SetString("phone", Phone);
				HttpContext.Session.SetString("address", Address);

				successMessage = "Profile updated correctly";
			}
			else if (submitButton.Equals("password"))
			{
				// validate Password and ConfirmPassword
				if (Password == null || Password.Length < 5 || Password.Length > 50)
				{
					errorMessage = "Password length should be between 5 and 50 characters";
					return;
				}

				if (ConfirmPassword == null || !ConfirmPassword.Equals(Password))
				{
					errorMessage = "Password and Confirm Password do not match";
					return;
				}

				// update the password in the database
				try
				{
					using (SqlConnection connection = new SqlConnection(connectionString))
					{
						connection.Open();

						string sql = "UPDATE users SET password=@password WHERE id=@id";

						int? id = HttpContext.Session.GetInt32("id");

						var passwordHasher = new PasswordHasher<IdentityUser>();
						string hashedPassword = passwordHasher.HashPassword(new IdentityUser(), Password);

						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("@password", hashedPassword);
							command.Parameters.AddWithValue("@id", id);

							command.ExecuteNonQuery();
						}
					}
				}
				catch (Exception ex)
				{
					errorMessage = ex.Message;
					return;
				}

				successMessage = "Password updated correctly";
			}

			//successMessage = "Profile or password updated correctly";
		}

		public IActionResult OnPostProfile()
		{
			// hapus validasi password
			ModelState.Remove("Password");
			ModelState.Remove("ConfirmPassword");

			if (!ModelState.IsValid)
			{
				errorMessage = "Profile validation failed";
				return Page();
			}

			int? id = HttpContext.Session.GetInt32("id");
			if (id == null)
			{
				return RedirectToPage("/Auth/Login");
			}

			try
			{
				Console.WriteLine("action : >>>>>> " + "profile");
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=netshoprazor_db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql = "UPDATE users SET firstname=@firstname, lastname=@lastname, " +
						"email=@email, phone=@phone, address=@address WHERE id=@id";

					//int? id = HttpContext.Session.GetInt32("id");
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname", Firstname);
						command.Parameters.AddWithValue("@lastname", Lastname);
						command.Parameters.AddWithValue("@email", Email);
						command.Parameters.AddWithValue("@phone", Phone);
						command.Parameters.AddWithValue("@address", Address);
						command.Parameters.AddWithValue("@id", id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return Page();
			}

			// update the session data
			HttpContext.Session.SetString("firstname", Firstname);
			HttpContext.Session.SetString("lastname", Lastname);
			HttpContext.Session.SetString("email", Email);
			HttpContext.Session.SetString("phone", Phone);
			HttpContext.Session.SetString("address", Address);

			successMessage = "Profile updated correctly";
			return Page();
		}
	
		public IActionResult OnPostPassword()
		{
			// hapus validasi profile
			ModelState.Remove("Firstname");
			ModelState.Remove("Lastname");
			ModelState.Remove("Email");
			ModelState.Remove("Phone");
			ModelState.Remove("Address");

			int? id = HttpContext.Session.GetInt32("id");
			if (id == null)
			{
				return RedirectToPage("/Auth/Login");
			}

			if (!ModelState.IsValid)
			{
				errorMessage = "Password validation failed";
				return Page();
			}

			// validate Password and ConfirmPassword
			if (Password == null || Password.Length < 5 || Password.Length > 50)
			{
				errorMessage = "Password length should be between 5 and 50 characters";
				return Page();
			}

			if (ConfirmPassword == null || !ConfirmPassword.Equals(Password))
			{
				errorMessage = "Password and Confirm Password do not match";
				return Page();
			}

			// update the password in the database
			try
			{
				Console.WriteLine("action : >>>>>> " + "password");
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=netshoprazor_db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql = "UPDATE users SET password=@password WHERE id=@id";

					var passwordHasher = new PasswordHasher<IdentityUser>();
					string hashedPassword = passwordHasher.HashPassword(new IdentityUser(), Password);

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@password", hashedPassword);
						command.Parameters.AddWithValue("@id", id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return Page();
			}

			successMessage = "Password updated correctly";
			return Page();
		}
	}
}
