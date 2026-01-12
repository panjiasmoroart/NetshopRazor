
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetshopRazor.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }

        // contact form - traditional validation
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";

        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnPost()
        {
            FirstName = Request.Form["FirstName"];
            LastName = Request.Form["LastName"];
            Email = Request.Form["Email"];
            Phone = Request.Form["Phone"];
            Subject = Request.Form["Subject"];
            Message = Request.Form["Message"];

            // check if any required field is empty
            if (FirstName.Length == 0 || LastName.Length == 0 || Email.Length == 0  || Subject.Length == 0 || Message.Length == 0)
            {
                ErrorMessage = "Please fill all required fields";
                return;
            }

            // Add this message to the database 

            // Send Confirmation Email to the client 

            SuccessMessage = "Your message has been received correctly";



			FirstName = "";
			LastName = "";
			Email = "";
			Phone = "";
			Subject = "";
			Message = "";
		}
	}
}
