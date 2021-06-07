using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SportWiz66.Data;
using SportWiz66.Models;

namespace SportWiz66.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly SportWiz66Context _context;

        public ShoppingCartsController(SportWiz66Context context)
        {
            _context = context;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                         ShoppingCart&Checokout:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //View your cart
        public async Task<IActionResult> Index()
        {
            var cart = GetShoppingCart();

            //if entering the cart and one items became out of stock , we implement here and not in ProductsController beacuase product can be in out of stock and then back, we dont want to delete for good
            var itemsList = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id).Include(c => c.Product);
            foreach (var item in itemsList)
                if (item.Product.InStock == false)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();
                }

            ViewBag.Total = GetTotalPrice();
            return View(await _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id).Include(c => c.Product).ToListAsync());
        }


        // View Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            //if u try to checkout with 0 items
            var cart = GetShoppingCart();
            if (GetNumOfItems() == 0)
                return RedirectToAction(nameof(Index));

            //if entering the checkout and one items became out of stock
            var itemsList = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id).Include(c => c.Product);
            foreach (var item in itemsList)
                if (item.Product.InStock == false)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

            return View();
        }


        //return table summary[html] of the shoppingcart(used in checkout) , because we using Order, cartItem, ShoppingCart controllers x3 and 1 view!
        [Authorize]
        public async Task<IActionResult> Table()
        {
            var cart = GetShoppingCart();
            ViewBag.Total = GetTotalPrice();
            return View(await _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id).Include(c => c.Product).ToListAsync());
        }


        // POST: Create Orders
        //Create an Order and OredDetails (copy the shoppingcart + get adress from user)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Checkout([Bind("Id,FirstName,LastName,Email,StreetAddress,ZipCode,City,Country,PhoneNumber,CreditCardNum")] Order order)
        {
            //if u try to checkout with 0 items
            var NumOfItems = GetNumOfItems();
            var cart = GetShoppingCart();
            if (NumOfItems == 0)
                return RedirectToAction(nameof(Index));


            //if one product is out of stock or deleted let view the cart again
            var itemsList = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id).Include(c => c.Product);
            foreach (var item in itemsList)
                if (item.Product.InStock == false || item.Product == null)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }



            string accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            order.AssociatedAccount = _context.Account.FirstOrDefault(i => i.Username == accountUserName);

            order.OrderDate = DateTime.Now;
            order.TotalPrice = GetTotalPrice();
            order.Status = OrderStatus.Processing;
            order.NumOfItems = NumOfItems;

            if (ModelState.IsValid)
            {
                _context.Add(order);

                //coping ShoppingCartItem into OrderDetail
                var cartItems = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart == cart).Include(p => p.Product);
                foreach (ShoppingCartItem item in cartItems)
                {
                    OrderDetail orderItem = new OrderDetail
                    {
                        AssociatedOrder = order,
                        Product = item.Product,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    };
                    _context.Add(orderItem);
                    item.Product.TotalPurchases += item.Quantity;
                    _context.Remove(item);
                }
                _context.Remove(cart);
                _context.SaveChanges();
                return RedirectToAction("MyOrderDetails", "Orders", order);
            }
            return View(order);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////









        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                    ADD, Update, Empty, Cart Items
        ///*Mehods Should Be In Sync!!!                                                    
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Get [Product ID] and add it as ShoppingCartItem to ShoppingCart, if already there ->Quantity++  (Limit to 5 items from the same product!)
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Product.FirstOrDefault(i => i.Id == productId);
            if (product == null)
                return Json("Product Doesnt Exists!!!");

            if (product.InStock == false)
                return Json("Sorry, Product is out of stock.");

            var cart = GetShoppingCart();

            //checking if ShoppingCart item for this product exists, If no => create ShoppingCartItem and place inside ShoppingCart
            var cartItem = _context.ShoppingCartItem.Include(p => p.Product).FirstOrDefault(i => i.AssociatedShoppingCart.Id == cart.Id && i.Product.Id == productId);
            if (cartItem == null)
            {
                //if reaced here: Products exists, ShoppingCartItem doesnt => Create ShoppingCartItem for this product!
                cartItem = new ShoppingCartItem { Product = product, UnitPrice = product.Price, AssociatedShoppingCart = cart };
                _context.ShoppingCartItem.Add(cartItem);
                _context.SaveChanges();
            }



            //If reached here: The ShoppingCartItem realted to the product is already inside the cart => update Quantity, MAX TO 5!
            if (cartItem.Quantity + 1 > 5)
            {

                return Json("Sorry, we cannot add another " + cartItem.Product.Name + " to your cart as you've already added the maximum amount of 5");
            }
            else
            {
                cartItem.Quantity++;
                _context.SaveChanges();

                return Json(cartItem.Product.Name + " added to your cart");

            }
        }



        //Get [CartItem ID] and add update its quantity, quantity per product for customer is 5!
        public IActionResult UpdateItemQuantity(int cartItemId, int quantity)
        {
            if (quantity > 5 || quantity < 0)
                return Json("Wrong Quantity");


            var cart = GetShoppingCart();

            //checking if ShoppingCart item exists and its inside the cart.
            var cartItem = _context.ShoppingCartItem.Include(p => p.Product).FirstOrDefault(i => i.AssociatedShoppingCart.Id == cart.Id && i.Id == cartItemId);
            if (cartItem == null)//item not exist => put item inside ShoppingCart
                return Json("NOT EXISTS!");

            //If we reached here: the item inside the cart => and we need to update the Quantity
            if (quantity == 0)
            {
                _context.ShoppingCartItem.Remove(cartItem);
                _context.SaveChanges();
                return Json(cartItem.Product.Name.ToString() + "Removed!");
            }
            else
            {
                cartItem.Quantity = quantity;
                _context.SaveChanges();
                return Json(cartItem.Product.Name.ToString() + "Update!");
            }
        }


        //Empty The Cart, DA!
        public IActionResult EmptyCart()
        {
            var cart = GetShoppingCart();
            //returns a list of all CartItems that Associated to Customer ShoppingCart
            var cartItems = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id);
            foreach (var item in cartItems)
                _context.ShoppingCartItem.Remove(item);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////









        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                         GET TotalPrice and Number OF Items Methods
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //returns Total price of all items
        public decimal GetTotalPrice()
        {
            var cart = GetShoppingCart();
            var cartItems = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id);
            decimal total = 0;
            foreach (var item in cartItems)
                total += item.UnitPrice * item.Quantity;

            return total;
        }

        //returns Num of items in the cart, used for counter and shoppingcartindex
        public int GetNumOfItems()
        {
            var cart = GetShoppingCart();
            var cartItems = _context.ShoppingCartItem.Where(i => i.AssociatedShoppingCart.Id == cart.Id);
            int numOfItems = 0;
            foreach (var item in cartItems)
                numOfItems += item.Quantity;

            return numOfItems;
        }







        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                        GetShoppingCart Using Cookies
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///Returns customer ShoppingCart according to GUID(uniqueID) placed in cookieI, f don't have one: create one! 
        private ShoppingCart GetShoppingCart()
        {
            //retrive GUID from cookie and look in DB for the ShoppingCart
            string cartGUID = HttpContext.Request.Cookies["ShoppingCartCookie"];
            var customerShoppingCart = _context.ShoppingCart.FirstOrDefault(i => i.AccountSessionID == cartGUID);

            // if the shoppingcart GUID doesn't exist in the cookie or no GUID like that in DB -> generate a new shoppingcart (old 1 may be still in DB)
            if (cartGUID == null || customerShoppingCart == null)
            {
                //if no GUID in cookie -> create cookie
                if (cartGUID == null)
                {
                    cartGUID = Guid.NewGuid().ToString();
                    CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
                    HttpContext.Response.Cookies.Append("ShoppingCartCookie", cartGUID, option); //seed cookie
                }

                //create ShoppingCart assosiated with the GUID
                customerShoppingCart = new ShoppingCart { AccountSessionID = cartGUID, DateCreated = DateTime.Now, LastModified = DateTime.Now };
                _context.ShoppingCart.Add(customerShoppingCart);
            }
            customerShoppingCart.LastModified = DateTime.Now;
            _context.SaveChanges();
            return customerShoppingCart;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    }

}

