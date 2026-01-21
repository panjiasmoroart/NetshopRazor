using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

		public void OnGet()
        {
        }
    }
}
