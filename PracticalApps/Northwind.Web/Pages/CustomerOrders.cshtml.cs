using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Northwind.Web.Pages;

public class CustomerOrdersModel : PageModel
{
	public Customer? Customer;

	private NorthwindContext db;

	public CustomerOrdersModel(NorthwindContext db)
	{
		this.db = db;
	}

	public void OnGet()
	{
		string id = HttpContext.Request.Query["id"];

		Customer = db.Customers.Include(c => c.Orders)
			.SingleOrDefault(c => c.CustomerId == id);
	}
}
