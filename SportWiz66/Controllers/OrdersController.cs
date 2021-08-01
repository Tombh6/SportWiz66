using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportWiz66.Data;
using SportWiz66.Models;
using Newtonsoft.Json;

namespace SportWiz66.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SportWiz66Context _context;

        public OrdersController(SportWiz66Context context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                                 Customer Area:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // show my orders(index for specific account)
        public async Task<IActionResult> MyOrders()
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") == null)
                return RedirectToAction("Login", "Accounts");

            var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            var account = await _context.Account.FirstOrDefaultAsync(m => m.Username == accountUserName);

            return View(await _context.Order.Where(i => i.AssociatedAccount == account).ToListAsync());
        }

        // show my order(order for specific account) get Detail ID!
        public async Task<IActionResult> MyOrderDetails(int? id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") == null)
                return RedirectToAction("Login", "Accounts");

            var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            var account = await _context.Account.FirstOrDefaultAsync(m => m.Username == accountUserName);


            if (id == null)
                return NotFound();


            var order = await _context.Order.FirstOrDefaultAsync(m => (m.Id == id) && (m.AssociatedAccount == account));

            if (order == null)
                return NotFound();


            //return View(order);
            return View(await _context.OrderDetail.Where(i => i.AssociatedOrder == order).Include(c => c.Product).ToListAsync());

        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                           Admin Mangement:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            //var list = _context.Order.GroupBy(s => s.Status).Select(i => new{ status = i.Key, count = i.Count() } ).ToList();

            return View(await _context.Order.Include(a => a.AssociatedAccount).ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            //return View(order);
            return View(await _context.OrderDetail.Where(i => i.AssociatedOrder == order).Include(c => c.Product).Include(c => c.AssociatedOrder.AssociatedAccount).ToListAsync());
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(OrderStatus)));
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,TotalPrice,Status,FirstName,LastName,Email,StreetAddress,ZipCode,City,Country,PhoneNumber,CreditCardNum,NumOfItems")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), order);
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            var orderDetails = _context.OrderDetail.Where(i => i.AssociatedOrder.Id == id);

            //we also deleting all orderDetails assosiated with the order
            foreach (var item in orderDetails)
                _context.OrderDetail.Remove(item);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }




        // Filter for Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(string term)
        {
            List<Order> orders;

            if (term != null)
                orders = await _context.Order.Where(c => c.Id.ToString().Contains(term) ||
                                                         c.TotalPrice.ToString().Contains(term) || c.NumOfItems.ToString().Contains(term)
                                                         ).ToListAsync();
            else
                orders = await _context.Order.ToListAsync();


            var accounts = await _context.Account.ToListAsync();
            var query = from order in orders
                        join account in accounts
                        on order.AssociatedAccount.Id equals account.Id
                        select new
                        {
                            id = order.Id,
                            orderdate = order.OrderDate.ToShortDateString(),
                            totalprice = order.TotalPrice,
                            numofitems = order.NumOfItems,
                            status = order.Status.ToString(),
                            email = account.Email,
                            username = account.Username,
                        };

            return Json(query);
        }


    }
}
