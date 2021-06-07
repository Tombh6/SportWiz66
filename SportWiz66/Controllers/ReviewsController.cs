using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SportWiz66.Data;
//using SportWiz66.Migrations;
using SportWiz66.Models;

namespace SportWiz66.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly SportWiz66Context _context;

        public ReviewsController(SportWiz66Context context)
        {
            _context = context;
        }


        //writer: DaViD Manshari at 4am :(
        //returns 5 reviews of certain product including the username to the JS ProductDetails
        public async Task<IActionResult> Index(int productId, int skipCount, int takeCount)
        {
            var reviews = await _context.Review.Where(c => c.Product.Id == productId).ToListAsync();
            reviews.Reverse();
            var accounts = await _context.Account.ToListAsync(); ;
            var query =
                from review in reviews
                join account in accounts
                on review.SentBy.Id equals account.Id
                select new
                {
                    title = review.Title,
                    content = review.Content,
                    date = review.Date.ToShortDateString(),
                    username = account.Username,
                    id = review.Id,
                };

            return Json(query.Skip(skipCount).Take(takeCount));
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] Review review, int productId)
        {
            review.Date = DateTime.Now;

            string accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            review.SentBy = _context.Account.FirstOrDefault(i => i.Username == accountUserName);
            review.Product = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }




        [Authorize(Roles = "Admin")]
        public int Delete(int id)
        {
            var review = _context.Review.FirstOrDefault(r => r.Id == id);

            if (review == null)
                return 0;
            _context.Review.Remove(review);
            _context.SaveChanges();
            return 1;
        }


    }
}
