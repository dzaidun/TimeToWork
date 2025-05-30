using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeToWork.Data;
using TimeToWork.Models;

namespace TimeToWork.Views.PlaceOfWorks
{
    public class PlaceOfWorksController : Controller
    {
        private readonly TimeToWorkContext _context;

        public PlaceOfWorksController(TimeToWorkContext context)
        {
            _context = context;
        }

        // GET: PlaceOfWorks
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlaceOfWork.OrderBy(i => i.Location).ToListAsync());
        }


        // GET: PlaceOfWorks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlaceOfWorks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaceOfWorkID,Location")] PlaceOfWork placeOfWork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(placeOfWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(placeOfWork);
        }

        // GET: PlaceOfWorks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlaceOfWork == null)
            {
                return NotFound();
            }

            var placeOfWork = await _context.PlaceOfWork.FindAsync(id);
            if (placeOfWork == null)
            {
                return NotFound();
            }
            return View(placeOfWork);
        }

        // POST: PlaceOfWorks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaceOfWorkID,Location")] PlaceOfWork placeOfWork)
        {
            if (id != placeOfWork.PlaceOfWorkID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placeOfWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceOfWorkExists(placeOfWork.PlaceOfWorkID))
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
            return View(placeOfWork);
        }

        // GET: PlaceOfWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlaceOfWork == null)
            {
                return NotFound();
            }

            var placeOfWork = await _context.PlaceOfWork
                .FirstOrDefaultAsync(m => m.PlaceOfWorkID == id);
            if (placeOfWork == null)
            {
                return NotFound();
            }

            return View(placeOfWork);
        }

        // POST: PlaceOfWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlaceOfWork == null)
            {
                return Problem("Entity set 'TimeToWorkContext.PlaceOfWork'  is null.");
            }
            var placeOfWork = await _context.PlaceOfWork.FindAsync(id);
            if (placeOfWork != null)
            {
                _context.PlaceOfWork.Remove(placeOfWork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceOfWorkExists(int id)
        {
            return _context.PlaceOfWork.Any(e => e.PlaceOfWorkID == id);
        }
    }
}
