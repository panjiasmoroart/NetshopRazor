
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetshopRazor.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
			//LoadSubjectList();
		}

		[BindProperty]
		[Required(ErrorMessage = "The First Name is required")]
		[Display(Name = "First Name*")]
		public string FirstName { get; set; } = "";

		[BindProperty, Required(ErrorMessage = "The Last Name is required")]
		[Display(Name = "Last Name*")]
		public string LastName { get; set; } = "";

		[BindProperty, Required(ErrorMessage = "The Email is required")]
		[EmailAddress]
		[Display(Name = "Email*")]
		public string Email { get; set; } = "";

		[BindProperty]
		public string? Phone { get; set; } = "";

		[BindProperty, Required]
		[Display(Name = "Subject*")]
		public string Subject { get; set; } = "";

		[BindProperty, Required(ErrorMessage = "The Message is required")]
		[MinLength(5, ErrorMessage = "The Message should be at least 5 characters")]
		[MaxLength(1024, ErrorMessage = "The Message should be less than 1024 characters")]
		[Display(Name = "Message*")]
		public string Message { get; set; } = "";

		public List<SelectListItem> SubjectList { get; } = new List<SelectListItem>
		{
			new SelectListItem { Value = "Order Status", Text = "Order Status" },
			new SelectListItem { Value = "Refund Request", Text = "Refund Request" },
			new SelectListItem { Value = "Job Application", Text = "Job Application"  },
			new SelectListItem { Value = "Other", Text = "Other"  },
		};


		public string SuccessMessage { get; set; } = "";
		public string ErrorMessage { get; set; } = "";

		public void OnPost()
        {
			//LoadSubjectList();

			// check if any required field is empty
			if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill all required fields";
                return;
            }

			if (Phone == null) Phone = "";

            // Add this message to the database 

            // Send Confirmation Email to the client 

            SuccessMessage = "Your message has been received correctly";

			FirstName = "";
			LastName = "";
			Email = "";
			Phone = "";
			Subject = "";
			Message = "";

			ModelState.Clear();
		}
	}
}
