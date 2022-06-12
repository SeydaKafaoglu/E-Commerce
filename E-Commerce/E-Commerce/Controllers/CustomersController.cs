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

namespace E_Commerce.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ECommerceContext _context;

        public CustomersController(ECommerceContext context)
        {
            _context = context;
        }
        public IActionResult Login(string currentURL)
        {
            ViewData["currentUrl"] = currentURL;
            return View();
        }
        public void TransferCart(long customerId, ECommerceContext eCommerceContext, HttpContext httpContext, string? newcart = null)
        {
            byte i = 0;
            long productId;
            string? cart;
            if (newcart == null)
            {
                cart = Request.Cookies["cart"];
            }
            else
            {
                cart = newcart;
            }
            if (cart=="")
            {
                cart = null;
            }

            string cartItem;
            string[] cartItems, itemDetails;
            CookieOptions cookieOptions = new CookieOptions();
            OrderDetail orderDetail;
            Product product;

            Order order;
            if (httpContext.Session.GetString("order") == null)
            {
                order = new Order();
                order.AllDelivered = false;
                order.IsCart = true;
                order.Cancelled = false;
                order.CustomerId = customerId;
                order.PaymentComplete = false;
                order.TimeStamp = DateTime.Now; //siparişin verildiği an               
            }
            else
            {
                order = eCommerceContext.Orders.FirstOrDefault(o => o.OrderId == Convert.ToInt64(httpContext.Session.GetString("order")));

            }
            order.OrderDetails = new List<OrderDetail>();
            order.OrderPrice = 0;

            if (cart != null)
            {
                cartItems = cart.Split(',');
                for (i = 0; i < cartItems.Length; i++)
                {
                    orderDetail = new OrderDetail();
                    cartItem = cartItems[i];
                    itemDetails = cartItem.Split(':');
                    productId = Convert.ToInt64(itemDetails[0]);
                    product = eCommerceContext.Products.FirstOrDefault(m => m.ProductId == productId);//product Id yi almak için
                    orderDetail.Count = Convert.ToByte(itemDetails[1]);
                    orderDetail.Price = product.ProductPrice * orderDetail.Count;
                    orderDetail.Product = product;
                    order.OrderPrice += orderDetail.Price;
                    order.OrderDetails.Add(orderDetail);
                }
                if (httpContext.Session.GetString("order") == null)
                {
                    eCommerceContext.Add(order);
                    eCommerceContext.SaveChanges();
                    if (order.OrderId != 0)
                    {
                        httpContext.Session.SetString("order", order.OrderId.ToString());
                    }

                }
                else
                {
                    eCommerceContext.Update(order);
                    eCommerceContext.SaveChanges();
                }
            }
            else
            {
                if (httpContext.Session.GetString("order") != null)
                {
                    eCommerceContext.Remove(order);
                    eCommerceContext.SaveChanges();
                    httpContext.Session.Remove("order");
                }
            }

            //ViewData["product"] = cartProducts; 
            //ViewData["cartTotal"] = cartTotal;
        }
        public void ProcessLogin([Bind("CustomerEMail", "CustomerPassword")] E_Commerce.Customer customer, string currentURL)
        {

            var dbUser = _context.Customers.FirstOrDefault(m => m.CustomerEMail == customer.CustomerEMail); //var dbUser veritabanından gelicek oloan user.
            SHA256 sHA256;
            byte[] hashedPassword;
            byte[] userPassword;

            if (dbUser != null)
            {
                string controlPassword;
                sHA256 = SHA256.Create();
                userPassword = Encoding.Unicode.GetBytes(customer.CustomerEMail.Trim() + customer.CustomerPassword.Trim());
                hashedPassword = sHA256.ComputeHash(userPassword);
                controlPassword = BitConverter.ToString(hashedPassword).Replace("-", "");

                if (controlPassword == dbUser.CustomerPassword)
                {
                    this.HttpContext.Session.SetString("customer", dbUser.CustomerId.ToString());
                    //key and value olarak guest değişkeninde tutulur(quest=id). sadece giriş yapıldığı bilgisi yetmediği için yetki sessionu ekledik.                 

                    TransferCart(dbUser.CustomerId, _context, this.HttpContext);
                    Response.Redirect(currentURL);
                    return;
                }
            }
            Response.Redirect("/Login");//DEĞİŞECEK
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create(string currentURL, bool noPassword = false)
        {
            ViewData["noPassword"] = noPassword;
            ViewData["currentUrl"] = currentURL;
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,CustomerSurname,CustomerEMail,CustomerPhone,CustomerPassword,CustomerConfirmPassword,CustomerAddress,IsDeleted")] Customer customer, string currentURL)
        {
            SHA256 sHA256;
            byte[] hashedPassword;
            byte[] customerPassword;
            string controlPassword;
            if (ModelState.IsValid)
            {
                sHA256 = SHA256.Create();
                customerPassword = Encoding.Unicode.GetBytes(customer.CustomerEMail.Trim() + customer.CustomerPassword.Trim());
                hashedPassword = sHA256.ComputeHash(customerPassword);
                controlPassword = BitConverter.ToString(hashedPassword).Replace("-", "");
                customer.CustomerPassword = controlPassword;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                this.HttpContext.Session.SetString("customer", customer.CustomerId.ToString());
                TransferCart(customer.CustomerId, _context, this.HttpContext);
                return Redirect(currentURL);
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerId,CustomerName,CustomerSurname,CustomerEMail,CustomerPhone,CustomerPassword,CustomerAddress,IsDeleted")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ECommerceContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
