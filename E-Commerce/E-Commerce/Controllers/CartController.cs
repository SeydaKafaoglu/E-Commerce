using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Models;

namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        public struct CartProduct
        {
            public Product Product;
            public int Count;
            public float Total;
        }
        public string Add(long id)
        {
            DbContextOptionsBuilder<ECommerceContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
            ECommerceContext eCommerceContext = new ECommerceContext(dbContextOptionsBuilder.Options);
            //dbContextOptionsBuilder.UseSqlServer("Data Source=seyda-pc;Initial Catalog=ECommerce; Integrated Security=True");
            CustomersController customersController = new CustomersController(eCommerceContext);

            string cart = Request.Cookies["cart"]; //cookilerden cart ı alıyoruz.
            string[] cartItems;
            string[] itemDetails;
            short itemCount;
            string newCart = "";
            string cartItem;
            short totalCount = 0;
            bool ıtemExıst = false;
            CookieOptions cookieOptions = new CookieOptions();

            if (cart == null)
            {
                newCart = id.ToString() + ":1";
                totalCount = 1;

            }
            else
            {

                cartItems = cart.Split(','); //ürünleri birbirinden ayırıyor
                for (short i = 0; i < cartItems.Length; i++)
                {
                    cartItem = cartItems[i];
                    itemDetails = cartItem.Split(':'); //ürünün ıd si ve adedini ayırıyor.
                    itemCount = Convert.ToInt16(itemDetails[1]);
                    if (itemDetails[0] == id.ToString())
                    {
                        itemCount++;
                        ıtemExıst = true;
                    }
                    totalCount += itemCount;
                    newCart = newCart + itemDetails[0] + ":" + itemCount.ToString();
                    if (i < cartItems.Length - 1)
                    {
                        newCart = newCart + ",";
                    }
                }
                if (ıtemExıst == false)
                {
                    newCart = newCart + "," + id.ToString() + ":1";
                    totalCount++;
                }
            }
            cookieOptions.Path = "/";
            cookieOptions.Expires = DateTime.MaxValue;

            Response.Cookies.Append("cart", newCart, cookieOptions);
            if (this.HttpContext.Session.GetString("customer") != null)
            {
                customersController.TransferCart(Convert.ToInt64(this.HttpContext.Session.GetString("customer")),eCommerceContext,this.HttpContext,newCart);
            }
            return totalCount.ToString();

        }
        //public string ProductSubTotal(short id, short count)
        //{

        //    //DbContextOptionsBuilder<WaterContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<WaterContext>();
        //    //WaterContext waterContext = new WaterContext(dbContextOptionsBuilder.Options);

        //    //Areas.Admin.Controllers.ProductsController productsController = new Areas.Admin.Controllers.ProductsController(waterContext);

        //    //float productPrice;
        //    //float subTotal;

        //    ////productprice = get price from model by ıd
        //    ////subtotal=productprice *count
        //    //return subTotal.;
        //}
        public string CartTotal(long[] subTotals)
        {
            long cartTotal = 0;
            foreach (long subTotalsItem in subTotals)
            {
                cartTotal += subTotalsItem;
            }
            return cartTotal.ToString();
        }
        public IActionResult Index()
        {
            DbContextOptionsBuilder<ECommerceContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
            ECommerceContext eCommerceContext = new ECommerceContext(dbContextOptionsBuilder.Options);
            dbContextOptionsBuilder.UseSqlServer("Data Source=seyda-pc;Initial Catalog=ECommerce; Integrated Security=True");
            Areas.Seller.Controllers.ProductsController productsController = new Areas.Seller.Controllers.ProductsController(eCommerceContext);
            Product product;

            byte i = 0;
            long productId;
            string? cart = Request.Cookies["cart"];
            string cartItem;
            string[] cartItems, itemDetails;

            List<CartProduct> cartProducts = new List<CartProduct>();
            float cartTotal = 0;


            if (cart != null)
            {
                cartItems = cart.Split(',');

                for (i = 0; i < cartItems.Length; i++)
                {
                    cartItem = cartItems[i];
                    itemDetails = cartItem.Split(':');
                    CartProduct cartProduct = new CartProduct();
                    productId = Convert.ToInt64(itemDetails[0]);
                    product = productsController.Product(productId);
                    cartProduct.Product = product;
                    cartProduct.Count = Convert.ToInt32(itemDetails[1]);
                    cartProduct.Total = cartProduct.Count * product.ProductPrice;
                    cartProducts.Add(cartProduct);
                    cartTotal += cartProduct.Total;

                }
            }
            ViewData["product"] = cartProducts;
            ViewData["cartTotal"] = cartTotal;
            return View();
        }
        public string CalculateTotal(long id, byte count)
        {
            DbContextOptionsBuilder<ECommerceContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
            ECommerceContext eCommerceContext = new ECommerceContext(dbContextOptionsBuilder.Options);
            dbContextOptionsBuilder.UseSqlServer("Data Source=seyda-pc;Initial Catalog=ECommerce; Integrated Security=True");
            Areas.Seller.Controllers.ProductsController productsController = new Areas.Seller.Controllers.ProductsController(eCommerceContext);
            Product product = productsController.Product(id);

            float subTotal;

            subTotal = product.ProductPrice * count;
            ChangeCookie(id, count);
            return subTotal.ToString();
        }
        private void ChangeCookie(long id, byte count)
        {
            DbContextOptionsBuilder<ECommerceContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
            ECommerceContext eCommerceContext = new ECommerceContext(dbContextOptionsBuilder.Options);         
            CustomersController customersController = new CustomersController(eCommerceContext);


            string cart = Request.Cookies["cart"]; //cookilerden cart ı alıyoruz.
            string[] cartItems;
            string[] itemDetails;
            short itemCount;
            string newCart = "";
            string cartItem;
            short totalCount = 0;

            CookieOptions cookieOptions = new CookieOptions();

            cartItems = cart.Split(','); //ürünleri birbirinden ayırıyor
            for (short i = 0; i < cartItems.Length; i++)
            {
                cartItem = cartItems[i];
                itemDetails = cartItem.Split(':'); //ürünün ıd si ve adedini ayırıyor.
                itemCount = Convert.ToInt16(itemDetails[1]);
                if (itemDetails[0] == id.ToString())
                {
                    itemCount = count;

                }

                totalCount += itemCount;
                newCart = newCart + itemDetails[0] + ":" + itemCount.ToString();

                if (i < cartItems.Length - 1)
                {
                    newCart = newCart + ",";
                }
                if (itemCount == 0) //sepet 0 a düşerse cookieden çıkarma
                {
                    continue;
                }

            }
            if (newCart != "")
            {
                if (newCart.Substring(newCart.Length - 1) == ",")
                {
                    //newCart.Remove(newCart.Length-1);
                    newCart = newCart.Substring(0, newCart.Length - 1);
                }
            }
            else
            {
                Response.Cookies.Delete("cart");
                return;

            }

            cookieOptions.Path = "/";
            cookieOptions.Expires = DateTime.MaxValue;
            Response.Cookies.Append("cart", newCart, cookieOptions);
            if (this.HttpContext.Session.GetString("customer") != null)
            {
                customersController.TransferCart(Convert.ToInt64(this.HttpContext.Session.GetString("customer")), eCommerceContext, this.HttpContext,newCart);
            }


        }
        public void emptyBasket()
        {
            DbContextOptionsBuilder<ECommerceContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
            ECommerceContext eCommerceContext = new ECommerceContext(dbContextOptionsBuilder.Options);           
            CustomersController customersController = new CustomersController(eCommerceContext);
        
            Response.Cookies.Delete("cart");
            if (this.HttpContext.Session.GetString("customer") != null)
            {
                customersController.TransferCart(Convert.ToInt64(this.HttpContext.Session.GetString("customer")), eCommerceContext, this.HttpContext, "");
            }
            Response.Redirect("Index");
        }
    }
}
