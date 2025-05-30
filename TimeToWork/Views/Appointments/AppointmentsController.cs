using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using TimeToWork.Data;
using TimeToWork.Models;

namespace TimeToWork.Views.Appointments
{
    public class AppointmentsController : Controller
    {
        private readonly TimeToWorkContext _context;

        public AppointmentsController(TimeToWorkContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["ServiceSortParm"] = sortOrder == "Service" ? "service_desc" : "Service";
            ViewData["ServiceProviderSortParm"] = sortOrder == "ServiceProvider" ? "serviceProvider_desc" : "ServiceProvider";
            ViewData["PriceParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;
			ViewData["CurrentSort"] = sortOrder;

			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			var appointments = from s in _context.Appointments
							   .Include(i => i.Client)
							   .Include(e => e.Service)
                               .Include(q => q.ServiceProvider)
							   select s;

			if (!String.IsNullOrEmpty(searchString))
			{
				appointments = appointments.Where(s => s.Service.ServiceName.Contains(searchString) || 
                                                  s.Date.ToString().Contains(searchString) || 
                                                 (s.Client.LastName+" "+ s.Client.FirstName).Contains(searchString) || 
                                                 (s.ServiceProvider.LastName + " " + s.ServiceProvider.FirstName).Contains(searchString));
			}
			switch (sortOrder)
			{
                case "Name":
                    appointments = appointments.OrderBy(s => s.Client.LastName);
                    break;
                case "name_desc":
					appointments = appointments.OrderByDescending(s => s.Client.LastName);
					break;
				case "Date":
					appointments = appointments.OrderBy(s => s.Date);
					break;
				case "date_desc":
					appointments = appointments.OrderByDescending(s => s.Date);
					break;
				case "Service":
					appointments = appointments.OrderBy(s => s.Service.ServiceName);
					break;
				case "service_desc":
					appointments = appointments.OrderByDescending(s => s.Service.ServiceName);
					break;
                case "ServiceProvider":
                    appointments = appointments.OrderBy(s => s.ServiceProvider.LastName);
                    break;
                case "serviceProvider_desc":
                    appointments = appointments.OrderByDescending(s => s.ServiceProvider.LastName);
                    break;
                case "Price":
                    appointments = appointments.OrderBy(s => s.Service.Price);
                    break;
                case "Price_desc":
                    appointments = appointments.OrderByDescending(s => s.Service.Price);
                    break;
                default:
					appointments = appointments.OrderBy(s => s.Date);
					break;
			}

			int pageSize = 9;
			return View(await PaginatedList<Appointment>.CreateAsync(appointments.AsNoTracking(), pageNumber ?? 1, pageSize));
		}

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.ServiceProvider)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            System.TimeSpan duration = new System.TimeSpan(0, appointment.Service.ЕxecutionTimeHours, appointment.Service.ЕxecutionTimeMinutes, 0);
            ViewData["EndOfMeating"] = appointment.Date.Add(duration);

            return View(appointment);
        }

        private void PopulateServicesDropDownList(object selectedService = null)
        {
            var servicesQuery = from d in _context.Services
                                   orderby d.ServiceName
                                   select d;
            ViewBag.ServiceId = new SelectList(servicesQuery.AsNoTracking(), "ServiceId", "ServiceName", selectedService);
			ViewBag.SelectedService = selectedService;
        }

		private void PopulateClientDropDownList(object selectedClient = null)
		{
			var clientsQuery = from d in _context.Clients
								orderby d.LastName
								select d;
			ViewBag.ClientId = new SelectList(clientsQuery.AsNoTracking(), "ID", "FullName", selectedClient);
			ViewBag.SelectedClient = selectedClient;
		}

        private List<SelectListItem> GetService()
        {
            var lstServices = new List<SelectListItem>();

            List<Service> Services = _context.Services.OrderBy(s => s.ServiceName).ToList();

            lstServices = Services.Select(sr => new SelectListItem()
            {
                Value = sr.ServiceId.ToString(),
                Text = sr.ServiceName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Обрати послугу"
            };

            lstServices.Insert(0, defItem);
            return lstServices;
        }
        private List<SelectListItem> GetServiceProvider(int serviceId)
        {
            List<SelectListItem> lstServiceProvider = _context.ServiceAssignments
                .Where(p => p.ServiceID == serviceId)
                .OrderBy(n => n.ServiceProvider.LastName)
                .Select(n => 
                new SelectListItem
                {
                    Value = n.ServiceProvider.ID.ToString(),
                    Text = n.ServiceProvider.FullName
                }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Обрати виконавця"
            };
            lstServiceProvider.Insert(0, defItem);
            return lstServiceProvider;

        }

        [HttpGet]
        public JsonResult GetServiceProviderByService(int serviceId)
        {
            List<SelectListItem> serviceProvider = GetServiceProvider(serviceId);
            return Json(serviceProvider);
        }


        // GET: Appointments/Create
        public IActionResult Create()
        {
            PopulateClientDropDownList();
            ViewBag.ServiceId = GetService();
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,ServiceId,ServiceProviderID,ClientId,Date")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateClientDropDownList(appointment.ClientId);

			//ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", appointment.ServiceId);
			//ViewData["ServiceProviderID"] = new SelectList(_context.Services, "ServiceProviderID", "FullName", appointment.ServiceProviderID);
			return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ID", "FullName", appointment.ClientId);
            ViewData["ServiceId"] = GetService();
            ViewData["ServiceProviderID"] = GetServiceProvider(appointment.ServiceId);
			return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,ServiceId,ServiceProviderID,ClientId,Date")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ID", "FullName", appointment.ClientId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", appointment.ServiceId);
			ViewData["ServiceProviderID"] = new SelectList(_context.Services, "ServiceProviderID", "FullName", appointment.ServiceProviderID);
			return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.ServiceProvider)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'TimeToWorkContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointments?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }


        // GET: Appointments/Done/5
        public async Task<IActionResult> Done(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var done = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .Include(a => a.ServiceProvider)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (done == null)
            {
                return NotFound();
            }

            System.TimeSpan duration = new System.TimeSpan(0, done.Service.ЕxecutionTimeHours, done.Service.ЕxecutionTimeMinutes, 0);
            ViewData["EndOfMeating"] = done.Date.Add(duration);

            return View(done);
        }

        // POST: Appointments/Done/5
        [HttpPost, ActionName("Done")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoneConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'TimeToWorkContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);

            Done done = new Done();
            done.ServiceId = appointment.ServiceId;
            done.ClientId = appointment.ClientId;
            done.Date = appointment.Date;
            done.ServiceProviderID = appointment.ServiceProviderID;

            if (ModelState.IsValid)
            {
                _context.Add(done);
                await _context.SaveChangesAsync();
            }

            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
