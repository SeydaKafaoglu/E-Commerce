using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Areas.Admin.Models;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserContext _context;

        public HomeController(UserContext context)
        {
            _context = context;
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login([Bind("UserEMail", "UserPassword")]User user)
        {
            var dbUser = _context.Users.FirstOrDefault(m => m.UserEMail == user.UserEMail); //var dbUser veritabanından gelicek oloan user.
            SHA256 sHA256;
            byte[] hashedPassword;
            byte[] userPassword;

            if (dbUser != null)
            {
                string controlPassword;
                sHA256 = SHA256.Create();
                userPassword = Encoding.Unicode.GetBytes(user.UserEMail.Trim() + user.UserPassword.Trim());
                hashedPassword = sHA256.ComputeHash(userPassword);               
                controlPassword = BitConverter.ToString(hashedPassword).Replace("-","");
                 
                if (controlPassword==dbUser.UserPassword)
                {
                    this.HttpContext.Session.SetString("guest", dbUser.UserId.ToString());//key and value olarak guest değişkeninde tutulur(quest=id). sadece giriş yapıldığı bilgisi yetmediği için yetki sessionu ekledik.                 
                    this.HttpContext.Session.SetString("viewUsers", dbUser.ViewUsers.ToString()); //kullanıcı görüntüleme yetkisi sessionu tekrar veritabanına sormamak için.sorulduğunda yavaşlar
                    this.HttpContext.Session.SetString("createUsers", dbUser.CreateUser.ToString());
                    this.HttpContext.Session.SetString("deleteUsers", dbUser.DeleteUser.ToString());
                    this.HttpContext.Session.SetString("editUsers", dbUser.EditUser.ToString());
                   
                    this.HttpContext.Session.SetString("viewSellers", dbUser.ViewSellers.ToString()); //kullanıcı görüntüleme yetkisi sessionu tekrar veritabanına sormamak için.sorulduğunda yavaşlar
                    this.HttpContext.Session.SetString("createSellers", dbUser.CreateSeller.ToString());
                    this.HttpContext.Session.SetString("deleteSellers", dbUser.DeleteSeller.ToString());
                    this.HttpContext.Session.SetString("editSellers", dbUser.EditSeller.ToString());

                    this.HttpContext.Session.SetString("viewCategories", dbUser.ViewCategories.ToString()); //kullanıcı görüntüleme yetkisi sessionu tekrar veritabanına sormamak için.sorulduğunda yavaşlar
                    this.HttpContext.Session.SetString("createCategory", dbUser.CreateCategory.ToString());
                    this.HttpContext.Session.SetString("deleteCategory", dbUser.DeleteCategory.ToString());
                    this.HttpContext.Session.SetString("editCategory", dbUser.EditCategory.ToString());
          
                    this.HttpContext.Session.SetString("deleteProducts", dbUser.DeleteProduct.ToString());
                    this.HttpContext.Session.SetString("editProduct", dbUser.EditProduct.ToString());





                    //return RedirectToAction(nameof(Login));
                    return RedirectToAction("Index", "Users"); //Yönlendirme için 3 yöntem
                    //Response.Redirect("~/Admin/Users/Index");
                }             
            }
            return RedirectToAction("Index"); //giriş yapamazsa

        }
    }
}
