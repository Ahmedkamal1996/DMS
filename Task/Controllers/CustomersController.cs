using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DMSTask.BLL.Helpers;
using DMSTask.DAL.Database;
using DMSTask.DAL.Models;
using DMSTask.BLL.Services;
using DMSTask.BLL.ViewModel;

namespace DMSTask.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomerServices _customerServices;

        public CustomersController(ApplicationDbContext context,CustomerServices customerServices)
        {
            _context = context;
            _customerServices = customerServices;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string filter)
        {
            ViewData["Filter"] = filter;

            var customers = _context.Customers.Include(c => c.User).AsQueryable();

            if(!string.IsNullOrWhiteSpace(filter))
            {
                customers = customers.Where(x =>
                    x.CustomerDescriptionAr!.Contains(filter) || 
                    x.CustomerDescriptionEn!.Contains(filter));
            }

            return View(await customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _customerServices.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( CustomerVM customer)
        {
            if (ModelState.IsValid)
            {
                _customerServices.Save(customer);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customer.UserId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public  IActionResult Edit(int? id)
        {

            var customer =  _customerServices.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customer.UserId);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( CustomerVM customer)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _customerServices.Save(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerCode))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customer.UserId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _customerServices.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _customerServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerCode == id);
        }
    }
}
