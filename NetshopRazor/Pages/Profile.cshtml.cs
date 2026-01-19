using System.ComponentModel.DataAnnotations;
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
			Phone = HttpContext.Session.GetString("phone");
			Address = HttpContext.Session.GetString("address") ?? "";
		}

		public void OnPost()
		{
			if (!ModelState.IsValid)
			{
				errorMessage = "Data validation failed";
				return;
			}

			// successful data validation
			if (Phone == null) Phone = "";


			// update the user profile or the password

			successMessage = "Profile or password updated correctly";
		}

	}
}
