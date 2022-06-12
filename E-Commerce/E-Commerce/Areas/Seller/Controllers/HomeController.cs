using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce;
using E_Commerce.Models;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class HomeController : Controller
    {
        private readonly ECommerceContext _context;

        public HomeController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: Seller/Home
        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Login([Bind("SellerEMail", "SellerPassword")] E_Commerce.Seller seller )
        {

            var dbUser = _context.Sellers.FirstOrDefault(m => m.SellerEMail == seller.SellerEMail); //var dbUser veritabanından gelicek oloan user.
            SHA256 sHA256;
            byte[] hashedPassword;
            byte[] userPassword;

            if (dbUser != null)
            {
                string controlPassword;
                sHA256 = SHA256.Create();
                userPassword = Encoding.Unicode.GetBytes(seller.SellerEMail.Trim() + seller.SellerPassword.Trim());
                hashedPassword = sHA256.ComputeHash(userPassword);
                controlPassword = BitConverter.ToString(hashedPassword).Replace("-", "");

                if (controlPassword == dbUser.SellerPassword)
                {
                    this.HttpContext.Session.SetInt32("merchant", dbUser.SellerId);//key and value olarak guest değişkeninde tutulur(quest=id). sadece giriş yapıldığı bilgisi yetmediği için yetki sessionu ekledik.                 
                    return RedirectToAction("Index", "Products");

                }
               
            }
            return RedirectToAction("Index");
        }


    }
}
