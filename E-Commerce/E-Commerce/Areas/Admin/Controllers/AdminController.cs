using Microsoft.AspNetCore.Mvc;


namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
       
       public AdminController(HttpContext httpContext)
        {
           
            if (httpContext.Session.GetString("quest") == null)
            {
                Response.Redirect("~/Admin");
            }

        }
    }
}
