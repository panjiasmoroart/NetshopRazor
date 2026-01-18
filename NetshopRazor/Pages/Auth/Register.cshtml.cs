using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetshopRazor.MyHelpers;

namespace NetshopRazor.Pages.Auth
{
	[BindProperties]
	public class RegisterModel : PageModel
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

		[Required(ErrorMessage = "Password is required")]
		[StringLength(50, ErrorMessage = "Password must be between 5 and 50 characters", MinimumLength = 5)]
		public string Password { get; set; } = "";

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
		public string ConfirmPassword { get; set; } = "";


		public string errorMessage = "";
		public string successMessage = "";

		public void OnGet()
		{
		}

		public void OnPost()
		{
			if (!ModelState.IsValid)
			{
				errorMessage = "Data validation failed";
				return;
			}

			// successfull data validation
			if (Phone == null) Phone = "";

			// add the user details to the database
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=netshoprazor_db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "INSERT INTO users " +
					"(firstname, lastname, email, phone, address, password, role) VALUES " +
					"(@firstname, @lastname, @email, @phone, @address, @password, 'client');";

					var passwordHasher = new PasswordHasher<IdentityUser>();
					string hashedPassword = passwordHasher.HashPassword(new IdentityUser(), Password);

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname", Firstname);
						command.Parameters.AddWithValue("@lastname", Lastname);
						command.Parameters.AddWithValue("@email", Email);
						command.Parameters.AddWithValue("@phone", Phone);
						command.Parameters.AddWithValue("@address", Address);
						command.Parameters.AddWithValue("@password", hashedPassword);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains(Email))
				{
					errorMessage = "Email address already used";
				}
				else
				{
					errorMessage = ex.Message;
				}

				return;
			}

			// send confirmation email to the user
			// send confirmation email to the user
			string username = Firstname + " " + Lastname;
			string subject = "Account created successfully";
			string message = "Dear " + username + ",\n\n" +
				"Your account has been created successfully.\n\n" +
				"Best Regards";
			EmailSender.SendEmail(Email, username, subject, message).Wait();

			// initialize the authenticated session => add the user details to the session data

			successMessage = "Account created successfully";
		}
	}

}