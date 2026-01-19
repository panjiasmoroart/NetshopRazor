using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetshopRazor.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
		[BindProperty, Required(ErrorMessage = "The Email is required"), EmailAddress]
		public string Email { get; set; } = "";

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

			// 1) create token, 2) save token in the database, 3) send token by email to the user

			successMessage = "Please check your email and click on the reset password link";
		}
	}
}
