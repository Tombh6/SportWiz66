using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportWiz66.Data;
using SportWiz66.Models;

namespace SportWiz66.Controllers
{
    public class ProductViewsController : Controller
    {
        private readonly SportWiz66Context _context;

        public ProductViewsController(SportWiz66Context context)
        {
            _context = context;
        }

        // GET: ProductViews
        public async Task<IActionResult> Index()
        {
            var sportWiz66Context = _context.ProductViews.Include(p => p.Account).Include(p => p.Product);
            return View(await sportWiz66Context.ToListAsync());
        }

        // GET: ProductViews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViews = await _context.ProductViews
                .Include(p => p.Account)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productViews == null)
            {
                return NotFound();
            }

            return View(productViews);
        }

        // GET: ProductViews/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Email");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: ProductViews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId")] ProductViews productViews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productViews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Email", productViews.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productViews.ProductId);
            return View(productViews);
        }

        // GET: ProductViews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViews = await _context.ProductViews.FindAsync(id);
            if (productViews == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Email", productViews.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productViews.ProductId);
            return View(productViews);
        }

        // POST: ProductViews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId")] ProductViews productViews)
        {
            if (id != productViews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productViews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductViewsExists(productViews.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Email", productViews.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productViews.ProductId);
            return View(productViews);
        }

        // GET: ProductViews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViews = await _context.ProductViews
                .Include(p => p.Account)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productViews == null)
            {
                return NotFound();
            }

            return View(productViews);
        }

        // POST: ProductViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productViews = await _context.ProductViews.FindAsync(id);
            _context.ProductViews.Remove(productViews);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductViewsExists(int id)
        {
            return _context.ProductViews.Any(e => e.Id == id);
        }
    }
}
