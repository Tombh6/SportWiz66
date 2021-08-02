using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportWiz66.Data;
using SportWiz66.Models;//TBH

namespace SportWiz66.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly SportWiz66Context _context;

        public CategoriesController(SportWiz66Context context)
        {
            _context = context;
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            return View(await _context.Category.Where(p => p.Name != "Not Found").ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);

            if (category == null || category.Name == "Not Found")
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (category.Name.ToLower() == "not found")
                ModelState.AddModelError("Name", "Please choose another name.");

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null || category.Name == "Not Found")
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (category.Name.ToLower() == "not found")
                ModelState.AddModelError("Name", "Please choose another name.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);

            if (category == null || category.Name == "Not Found")
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);

            //assosiated products will change theire category to "Not Found"
            var defaultCategory = _context.Category.FirstOrDefault(p => p.Name == "Not Found");
            var products = _context.Product.Where(p => p.Category == category);
            foreach (var product in products)
                product.Category = defaultCategory;


            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }



        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }




        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(string term)
        {
            List<Category> categories;

            if (term != null)
                categories = await _context.Category.Where(c => c.Id.ToString().Contains(term) || c.Name.Contains(term)).Where(p => p.Name != "Not Found").ToListAsync();

            else
                categories = await _context.Category.Where(p => p.Name != "Not Found").ToListAsync();


            var query = from category in categories
                        select new
                        {
                            id = category.Id,
                            name = category.Name,
                        };

            return Json(query);
        }

    }
}
