using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TimeToWork.Data;
using TimeToWork.Models;

namespace TimeToWork.Views.Dones
{
    public class DonesController : Controller
    {
        private readonly TimeToWorkContext _context;

        public DonesController(TimeToWorkContext context)
        {
            _context = context;
        }

        // GET: Dones
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["ServiceSortParm"] = sortOrder == "Service" ? "service_desc" : "Service";
            ViewData["ServiceProviderSortParm"] = sortOrder == "ServiceProvider" ? "serviceProvider_desc" : "ServiceProvider";
            ViewData["PriceParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            var done = from s in _context.Done
                               .Include(i => i.Client)
                               .Include(e => e.Service)
                               .Include(q => q.ServiceProvider)
                               select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                done = done.Where(s => s.Client.LastName.Contains(searchString) || s.Client.FirstName.Contains(searchString) || s.Service.ServiceName.Contains(searchString) || s.Date.ToString().Contains(searchString) || (s.Client.LastName + " " + s.Client.FirstName).Contains(searchString) || (s.ServiceProvider.LastName + " " + s.ServiceProvider.FirstName).Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    done = done.OrderByDescending(s => s.Client.LastName);
                    break;
                case "Date":
                    done = done.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    done = done.OrderByDescending(s => s.Date);
                    break;
                case "Service":
                    done = done.OrderBy(s => s.Service.ServiceName);
                    break;
                case "service_desc":
                    done = done.OrderByDescending(s => s.Service.ServiceName);
                    break;
                case "ServiceProvider":
                    done = done.OrderBy(s => s.ServiceProvider.LastName);
                    break;
                case "serviceProvider_desc":
                    done = done.OrderByDescending(s => s.ServiceProvider.LastName);
                    break;
                case "Price":
                    done = done.OrderBy(s => s.Service.Price);
                    break;
                case "Price_desc":
                    done = done.OrderByDescending(s => s.Service.Price);
                    break;
                default:
                    done = done.OrderBy(s => s.Date);
                    break;
            }

            return View(done);
        }

        // GET: Dones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Done == null)
            {
                return NotFound();
            }

            var done = await _context.Done
                .Include(d => d.Client)
                .Include(d => d.Service)
                .Include(d => d.ServiceProvider)
                .FirstOrDefaultAsync(m => m.DoneId == id);
            if (done == null)
            {
                return NotFound();
            }

            System.TimeSpan duration = new System.TimeSpan(0, done.Service.ЕxecutionTimeHours, done.Service.ЕxecutionTimeMinutes, 0);
            ViewData["EndOfMeating"] = done.Date.Add(duration);

            return View(done);
        }

        // GET: Dones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Done == null)
            {
                return NotFound();
            }

            var done = await _context.Done
                .Include(d => d.Client)
                .Include(d => d.Service)
                .Include(d => d.ServiceProvider)
                .FirstOrDefaultAsync(m => m.DoneId == id);
            if (done == null)
            {
                return NotFound();
            }

            return View(done);
        }

        // POST: Dones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Done == null)
            {
                return Problem("Entity set 'TimeToWorkContext.Done'  is null.");
            }
            var done = await _context.Done.FindAsync(id);
            if (done != null)
            {
                _context.Done.Remove(done);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoneExists(int id)
        {
            return _context.Done.Any(e => e.DoneId == id);
        }
    }
}
