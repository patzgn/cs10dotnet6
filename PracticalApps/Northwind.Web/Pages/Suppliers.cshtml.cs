using Microsoft.AspNetCore.Mvc; // [BindProperty], IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using Shared; // NorthwindContext

namespace Northwind.Web.Pages;

public class SuppliersModel : PageModel
{
	public IEnumerable<Supplier>? Suppliers { get; set; }

	private NorthwindContext db;

	public SuppliersModel(NorthwindContext injectedContext)
	{
		db = injectedContext;
	}

	public void OnGet()
	{
		ViewData["Title"] = "Northwind B2B - Suppliers";

		Suppliers = db.Suppliers
			.OrderBy(s => s.Country).ThenBy(s => s.CompanyName);
	}

	[BindProperty]
	public Supplier? Supplier { get; set; }

	public IActionResult OnPost()
	{
		if ((Supplier is not null) && ModelState.IsValid)
		{
			db.Suppliers.Add(Supplier);
			db.SaveChanges();
			return RedirectToPage("/suppliers");
		}
		return Page();
	}
}
