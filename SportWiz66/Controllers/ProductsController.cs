using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SportWiz66.Data;
using SportWiz66.Models;
using Tweetinvi;

namespace SportWiz66.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SportWiz66Context _context;

        public ProductsController(SportWiz66Context context)
        {
            _context = context;
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                         Products Main Page:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // View Products Page, no need to pass in view because JS
        public async Task<IActionResult> Index()
        {
            ViewBag.Brands = new SelectList(await _context.Brand.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Category.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            return View();
        }



        //returns newst products from DB to the JS ProductIndex to be inserted in the html(in JS we always take 6 and show them)
        //returns 6 products filtered by params
        //if sort is null we take newst 
        public async Task<IActionResult> getProducts(int modeId, int brandId, int categoryId, string sort, int skipCount, int takeCount)
        {
            var products = await _context.Product.Include(i => i.Category).Include(t => t.Brand).Where(p => p.InStock == true && p.Name != "Not Found").ToListAsync();

            //FILTERS
            ////////////////////////////////////////////////////////////////////
            if (brandId != 0)
                products.RemoveAll(b => b.Brand.Id != brandId);

            if (categoryId != 0)
                products.RemoveAll(c => c.Category.Id != categoryId);


            //by default the list ordered by newest
            if (sort == "low2high")
                products.Sort((a, b) => a.Price.CompareTo(b.Price));

            if (sort == "high2low")
            {
                products.Sort((a, b) => a.Price.CompareTo(b.Price));
                products.Reverse();
            }

            if (sort == "alphabetical")
                products.Sort((a, b) => a.Name.CompareTo(b.Name));


            if (sort == "newest" || sort == null)
                products.Sort((a, b) => b.Id.CompareTo(a.Id));

            if (modeId == 0)
            {
                var query =
                    from product in products
                    select new
                    {
                        id = product.Id,
                        imageUrl = product.ImageUrl,
                        name = product.Name,
                        price = product.Price,

                    };
                return Json(query.Skip(skipCount).Take(takeCount));
            }


            //
            //mode 1 special ALGORITHM 
            else
            {
                //get productVIEWS list 
                var productViewsDB = await _context.ProductViews.Where(p => p.Product.InStock == true && p.Product.Name != "Not Found").ToListAsync();

                //Filter registered user specific productVIEWS
                if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") != null)
                {
                    var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
                    var account = await _context.Account.FirstOrDefaultAsync(m => m.Username == accountUserName);

                    productViewsDB.RemoveAll(v => v.Account != account);
                }



                //groupby in productVIEWS to know how many time each account viewed the same product, and now we also know which category he most entering
                var CategoryViews = from list in productViewsDB
                                    group list by list.Product.Category into g
                                    select new { Category = g.Key, Count = g.Count() };

                //JOIN VIEWS WITH Products we filterd!
                var query = (from view in CategoryViews
                             join product in products
                             on view.Category equals product.Category
                             select new
                             {
                                 score = view.Count + (2 * product.TotalPurchases),
                                 id = product.Id,
                                 name = product.Name,
                                 price = product.Price,
                                 ImageUrl = product.ImageUrl,
                                 brand = product.Brand,
                                 category = product.Category,
                                 views = view.Count,
                                 orders = product.TotalPurchases
                             }).ToList().OrderBy(x => x.score).Reverse();

                //FINISHED CREATING SUGGESTION!, u can stop and check query here to watch items score :)



                //GET RESULT READY and trim:
                //its also possible we dont get any suggestion because no product is ever viewd or purchased, that case we return answer like mode 0
                if (query.Any())
                {
                    var result = from product in query
                                 select new
                                 {
                                     id = product.id,
                                     imageUrl = product.ImageUrl,
                                     name = product.name,
                                     price = product.price,

                                 };
                    return Json(result.Skip(skipCount).Take(takeCount));
                }

                else
                {
                    var result = from product in products
                                 select new
                                 {
                                     id = product.Id,
                                     imageUrl = product.ImageUrl,
                                     name = product.Name,
                                     price = product.Price,

                                 };
                    return Json(result.Skip(skipCount).Take(takeCount));
                }



            }

        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                         Products Detalis Page:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(b => b.Brand).Include(i => i.Reviews).ThenInclude(c => c.SentBy).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null || product.Name == "Not Found")
            {
                return NotFound();
            }

            //User logged in = > adding product to RecentlyViewd for Special Offers ALGORITHM
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") != null)
            {
                var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
                var account = await _context.Account.FirstOrDefaultAsync(m => m.Username == accountUserName);
                ProductViews newView = new ProductViews { Account = account, Product = product };
                _context.ProductViews.Add(newView);
                await _context.SaveChangesAsync();
            }

            return View(product);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                         SearchBox Method for Upper Nav:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> Search(string term)
        {
            var result = from p in _context.Product.Where(p => p.Name != "Not Found")
                         where p.Name.Contains(term)
                         select new { id = p.Id, label = p.Name, value = p.Id };


            return Json(await result.ToListAsync());
        }







        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                         ADMIN PAGES:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            return View(await _context.Product.Where(p => p.Name != "Not Found").Include(i => i.Category).Include(t => t.Brand).ToListAsync());
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// CREATE Methods:
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = new SelectList(await _context.Brand.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Category.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,FrontImageUrl,BackImageUrl,InStock")] Product product, int BrandId, int CategoryId)
        {


            product.TotalPurchases = 0;
            product.Brand = _context.Brand.First(b => b.Id == BrandId);
            product.Category = _context.Category.First(c => c.Id == CategoryId);

            if (product.ImageUrl == null)
                product.ImageUrl = "/images/Default.png";
            if (product.BackImageUrl == null)
                product.BackImageUrl = "/images/Default.png";
            if (product.FrontImageUrl == null)
                product.FrontImageUrl = "/images/Default.png";

            if (product.Brand == null)
                ModelState.AddModelError("Brand", "Please select a brand.");

            if (product.Category == null)
                ModelState.AddModelError("Category", "Please select a category.");

            if (product.Name.ToLower() == "not found")
                ModelState.AddModelError("Name", "Please choose another name.");

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = new SelectList(await _context.Brand.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Category.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            return View(product);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// EDIT Methods:
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null || product.Name == "Not Found")
            {
                return NotFound();
            }
            ViewBag.Brands = new SelectList(await _context.Brand.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Category.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,FrontImageUrl,BackImageUrl,InStock")] Product product, int BrandId, int CategoryId)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (product.ImageUrl == null)
                product.ImageUrl = "/images/Default.png";
            if (product.BackImageUrl == null)
                product.BackImageUrl = "/images/Default.png";
            if (product.FrontImageUrl == null)
                product.FrontImageUrl = "/images/Default.png";
            product.Brand = _context.Brand.FirstOrDefault(b => b.Id == BrandId);
            product.Category = _context.Category.FirstOrDefault(c => c.Id == CategoryId);



            if (product.Brand == null)
                ModelState.AddModelError("Brand", "Please select a brand.");

            if (product.Category == null)
                ModelState.AddModelError("Category", "Please select a category.");

            if (product.Name.ToLower() == "not found")
                ModelState.AddModelError("Name", "Please choose another name.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Admin));
            }
            ViewBag.Brands = new SelectList(await _context.Brand.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Category.Where(n => n.Name != "Not Found").ToListAsync(), "Id", "Name");
            return View(product);
        }




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// DELETE Methods:
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null || product.Name == "Not Found")
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);


            //assosiated OrderDetails will change theire Product to "Not Found"
            var defaultproduct = _context.Product.FirstOrDefault(p => p.Name == "Not Found");
            var orderDetails = _context.OrderDetail.Where(p => p.Product == product);
            foreach (var orderDetail in orderDetails)
                orderDetail.Product = defaultproduct;


            //Delete assosiated reviews and shoppingcart items
            _context.Review.RemoveRange(_context.Review.Where(p => p.Product == product));
            _context.ShoppingCartItem.RemoveRange(_context.ShoppingCartItem.Where(p => p.Product == product));
            _context.Product.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(string term)
        {
            List<Product> products;

            if (term != null)
                products = await _context.Product.Where(c => c.Id.ToString().Contains(term) || c.Name.Contains(term) ||
                                                         c.Price.ToString().Contains(term) || c.Description.Contains(term) || c.TotalPurchases.ToString().Contains(term) ||
                                                         c.Brand.Name.Contains(term) || c.Category.Name.Contains(term)).Where(p => p.Name != "Not Found").ToListAsync();
            else
                products = await _context.Product.Where(p => p.Name != "Not Found").ToListAsync();

            var brands = await _context.Brand.Where(p => p.Name != "Not Found").ToListAsync();
            var categories = await _context.Category.Where(p => p.Name != "Not Found").ToListAsync();

            var query = from product in products
                        join brand in brands on product.Brand.Id equals brand.Id
                        join category in categories on product.Category.Id equals category.Id
                        select new
                        {
                            id = product.Id,
                            name = product.Name,
                            price = product.Price,
                            description = product.Description,
                            brand = brand.Name,
                            category = category.Name,
                            instock = product.InStock,
                            totalpurchases = product.TotalPurchases,
                            img = product.ImageUrl,
                        };

            return Json(query);
        }


        //tweet and return tweerID
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Tweet(int id)
        {


            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null || product.Name == "Not Found")
            {
                return NotFound();
            }


            var userClient = new TwitterClient("K2CfPa9r79luL0GqkPADUu9uK", "fn7Gcz8xAkvyEzFinXNKKiW6wMyqyaQyyfn0tzaDQSwnCUXHDH", "1399847438501007362-J9rUgQ7KqK6GTHT3RRzBYthVJy9uOz", "sTrPGkvHcUrQzVXFiuKmXZFR5NC8YVju4q8XWFIRpZ7IA");
            var publishedTweet = await userClient.Tweets.PublishTweetAsync("We got " + product.Name);


            var tweet = await userClient.Tweets.GetTweetAsync(publishedTweet.Id); //get the tweet
            var oEmbedTweet = await userClient.Tweets.GetOEmbedTweetAsync(tweet); //make it embedded


            return Json(oEmbedTweet.HTML);
        }





        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics()
        {
            var products = await _context.Product.Where(p => p.InStock == true && p.Name != "Not Found").ToListAsync();

            return View(products);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StatsByOrders()
        {
            var products = await _context.Product.Where(p => p.InStock == true && p.Name != "Not Found").ToListAsync();

            var result = (from product in products
                          select new
                          {
                              Name = product.Name,
                              Value = product.TotalPurchases
                          }).ToList().OrderBy(x => x.Value).Reverse();


            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StatsByViews()
        {

            //Products list
            var products = await _context.Product.Where(p => p.InStock == true && p.Name != "Not Found").ToListAsync();

            //productVIEWS list 
            var productViewsDB = await _context.ProductViews.Where(p => p.Product.InStock == true && p.Product.Name != "Not Found").ToListAsync();
            //groupby in productVIEWS to know how many time each account viewed the same product.
            var CategoryViews = from list in productViewsDB
                                group list by list.Product into g
                                select new { Product = g.Key, Count = g.Count() };




            //JOIN VIEWS WITH Products we filterd!
            var result = (from view in CategoryViews
                          join product in products
                          on view.Product equals product
                          select new
                          {
                              Name = product.Name,
                              value = view.Count
                          }).ToList().OrderBy(x => x.value).Reverse();

            return Json(result);
        }


    }
}
