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
using DMSTask.DAL.Enums;
using DMSTask.BLL.Services;
using DMSTask.BLL.ViewModel;

namespace DMSTask.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderServices _orderServices;

        public OrdersController(ApplicationDbContext context, OrderServices orderServices)
        {
            _context = context;
            _orderServices = orderServices;
        }

        // GET: OrderHeaders
        public async Task<IActionResult> Index(string filter)
        {
            ViewData["Filter"] = filter;

            List<object> orderStatus = GetOrderStatus();

            ViewData["OrderStatus"] = new SelectList(orderStatus, "Id", "Value");

            var applicationDbContext = _context.OrderHeaders.Include(o => o.Customer)
                //.Where(x => x.Status != OrderStatus.Open)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(filter))
            {
                applicationDbContext = applicationDbContext.Where(x =>
                    x.Customer!.CustomerDescriptionAr!.Contains(filter) ||
                    x.Customer!.CustomerDescriptionEn!.Contains(filter)
                );
            }

            return View(await applicationDbContext.OrderByDescending(x => x.OrderDate).ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = _orderServices.Details(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            return View(orderHeader);
        }

        public IActionResult Create()
        {
            List<object> orderStatus = GetOrderStatus();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerCode", "DescriptionEn");
            ViewData["OrderStatus"] = new SelectList(orderStatus, "Id", "Value");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderHeaderVM orderHeader)
        {
            if (ModelState.IsValid)
            {
                _orderServices.Save(orderHeader);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerCode", "CustomerCode", orderHeader.CustomerId);
            return View(orderHeader);
        }

        // GET: OrderHeaders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = _orderServices.GetId(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            List<object> orderStatus = GetOrderStatus();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerCode", "DescriptionEn");
            ViewData["OrderStatus"] = new SelectList(orderStatus, "Id", "Value");

            return View(orderHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( OrderHeaderVM orderHeader)
        {
         
            if (ModelState.IsValid)
            {
                try
                {
                    _orderServices.Save(orderHeader);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderHeaderExists(orderHeader.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerCode", "CustomerCode", orderHeader.CustomerId);
            return View(orderHeader);
        }

        // GET: OrderHeaders/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = _orderServices.Show(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            return View(orderHeader);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _orderServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderHeaderExists(int id)
        {
            return _context.OrderHeaders.Any(e => e.Id == id);
        }

        private static List<object> GetOrderStatus()
        {
            var orderStatus = new List<object>();
            orderStatus.Add(new { Id = (int)OrderStatus.Open, Value = OrderStatus.Open.ToString() });
            orderStatus.Add(new { Id = (int)OrderStatus.Closed, Value = OrderStatus.Closed.ToString() });
            return orderStatus;
        }
    }
}
