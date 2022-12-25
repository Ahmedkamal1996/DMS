using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DMSTask.BLL.Helpers;
using DMSTask.DAL.Database;
using DMSTask.BLL.Repositories;
using DMSTask.DAL.Models;
using DMSTask.DAL.Enums;
using DMSTask.BLL.Services;
using DMSTask.BLL.ViewModel;

namespace DMSTask.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemServices _itemServices;
        private readonly IUserService _userService;

        public ItemsController(ApplicationDbContext context,
            IUserService userService, ItemServices itemServices)
        {
            _context = context;
            _userService = userService;
            _itemServices = itemServices;
        }

        #region Admin Actions
        // GET: Items
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index(string filter)
        {
            ViewData["Filter"] = filter;

            var items = _context.Items.Include(i => i.UnitOfMeasure).AsQueryable();

            if(!string.IsNullOrWhiteSpace(filter))
            {
                items = items.Where(x =>
                    x.Name!.Contains(filter) ||
                    x.Description!.Contains(filter) ||
                    x.UnitOfMeasure!.UOM!.Contains(filter)
                );
            }

            return View(await items.ToListAsync());
        }

        // GET: Items/Details/5
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemServices.Details(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Create()
        {
            ViewData["UOM"] = new SelectList(_context.UnitOfMeasures, "Id", "UOM");
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( ItemVM item)
        {
            if (ModelState.IsValid)
            {
                _itemServices.Save(item);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UOM"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", item.UOMId);
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemServices.GetId(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["UOM"] = new SelectList(_context.UnitOfMeasures, "Id", "UOM", item.UOMId);
            return View(item);
        }

        
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( ItemVM item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _itemServices.Save(item);
                }
                catch (DbUpdateConcurrencyException)
                {
  
                        return NotFound();       
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UOM"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", item.UOMId);
            return View(item);
        }

    
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemServices.Details(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _itemServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region User Actions
        // GET: AvaliableItems
        [Authorize(Roles = Roles.Customer)]
        public IActionResult AvaliableItems()
        {
            var items = _context.Items.Include(i => i.UnitOfMeasure)
                .Where(i => i.Quantity > 0)
                .AsQueryable();

            return View(items.ToList());
        }

        [Authorize(Roles = Roles.Customer)]
        public ActionResult AddToCard(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemServices.Details(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCardConfirm(int id, int quantity)
        {
            var customer =  _context.Customers.FirstOrDefault(x => x.UserId == _userService.GetUserId());
            _itemServices.AddItemToCart(id, quantity, customer);
            return RedirectToAction(nameof(AvaliableItems));
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult Card()
        {
            var customer =  _context.Customers.FirstOrDefault(x => x.UserId == _userService.GetUserId());

            var order = _itemServices.GetItemToCard(customer);

            return View(order);
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult RemoveFromCard(int orderDetailId)
        {

            var result = _itemServices.RemoveItemFromCard(orderDetailId);
            if (result == false)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Card));
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult SaveOrder(int orderId)
        {
            var customer =  _context.Customers.FirstOrDefault(x => x.UserId == _userService.GetUserId());

            var result = _itemServices.SaveOrder(orderId, customer);

            if (result == false) return NotFound();

            return RedirectToAction(nameof(AvaliableItems));
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult DisplayMyOrders()
        {
            var customer =  _context.Customers.FirstOrDefault(x => x.UserId == _userService.GetUserId());

            var orders = _itemServices.GetOrders(customer);

            return View(orders);
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult DisplayMyOrderDetails(int orderId)
        {
            var customer =  _context.Customers.FirstOrDefault(x => x.UserId == _userService.GetUserId());

            var order = _itemServices.GetOrderDetails(orderId, customer);

            return View(order);
        }
        #endregion
    }
}
