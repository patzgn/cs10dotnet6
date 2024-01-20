using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared;

namespace Northwind.Web.Pages;

public class CustomersModel : PageModel
{
	public ILookup<string?, Customer>? CustomersByCountry;

	private NorthwindContext db;

	public CustomersModel(NorthwindContext db)
	{
		this.db = db;
	}

	public void OnGet()
	{
		CustomersByCountry = db.Customers.ToLookup(x => x.Country);
	}
}
