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

namespace SportWiz66.Controllers
{
    public class BrandsController : Controller
    {
        private readonly SportWiz66Context _context;

        public BrandsController(SportWiz66Context context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            return View(await _context.Brand.Where(p => p.Name != "Not Found").ToListAsync());
        }


        // GET: Brands/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null || brand.Name == "Not Found")
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl")] Brand brand)
        {
            if (brand.ImageUrl == null)
                brand.ImageUrl = "/images/Default.png";

            if (brand.Name.ToLower() == "not found")
                ModelState.AddModelError("Name", "Please choose another name.");

            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FindAsync(id);
            if (brand == null || brand.Name == "Not Found")
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (brand.ImageUrl == null)
                brand.ImageUrl = "/images/Default.png";

            if (brand.Name.ToLower() == "not found")
                ModelState.AddModelError("Name", "Please choose another name.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }

        // GET: Brands/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null || brand.Name == "Not Found")
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brand.FindAsync(id);

            //assosiated products will change theire brand to "Not Found"
            var defaultBrand = _context.Brand.FirstOrDefault(p => p.Name == "Not Found");
            var products = _context.Product.Where(p => p.Brand == brand);
            foreach (var product in products)
                product.Brand = defaultBrand;


            _context.Brand.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        private bool BrandExists(int id)
        {
            return _context.Brand.Any(e => e.Id == id);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(string term)
        {
            List<Brand> brands;

            if (term != null)
                brands = await _context.Brand.Where(c => c.Id.ToString().Contains(term) || c.Name.Contains(term) ||
                                                         c.Description.Contains(term)).Where(p => p.Name != "Not Found").ToListAsync();

            else
                brands = await _context.Brand.Where(p => p.Name != "Not Found").ToListAsync();


            var query = from brand in brands
                        select new
                        {
                            id = brand.Id,
                            name = brand.Name,
                            description = brand.Description,
                        };

            return Json(query);
        }

    }
}