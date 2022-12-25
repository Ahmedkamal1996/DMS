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
    public class UnitOfMeasuresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfMeasureServices _unitOfMeasureServices;

        public UnitOfMeasuresController(ApplicationDbContext context, UnitOfMeasureServices unitOfMeasureServices)
        {
            _context = context;
            _unitOfMeasureServices = unitOfMeasureServices;
        }

        // GET: UnitOfMeasures
        public async Task<IActionResult> Index(string filter)
        {
            ViewData["Filter"] = filter;

            var data = _context.UnitOfMeasures.AsQueryable();

            if(!string.IsNullOrWhiteSpace(filter))
            {
                data = data.Where(x => 
                    x.Description!.Contains(filter) ||
                    x.UOM!.Contains(filter)
                );
            }

            return View(await data.ToListAsync());
        }

        // GET: UnitOfMeasures/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unitOfMeasure = _unitOfMeasureServices.GetId(id);
            if (unitOfMeasure == null)
            {
                return NotFound();
            }

            return View(unitOfMeasure);
        }

        // GET: UnitOfMeasures/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UnitOfMeasureVM unitOfMeasure)
        {
            if (ModelState.IsValid)
            {
                _unitOfMeasureServices.Save(unitOfMeasure);
                return RedirectToAction(nameof(Index));
            }
            return View(unitOfMeasure);
        }

        // GET: UnitOfMeasures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unitOfMeasure = await _context.UnitOfMeasures.FindAsync(id);
            if (unitOfMeasure == null)
            {
                return NotFound();
            }
            return View(unitOfMeasure);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UnitOfMeasureVM unitOfMeasure)
        {
         

            if (ModelState.IsValid)
            {
                try
                {
                   _unitOfMeasureServices.Save(unitOfMeasure);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitOfMeasureExists(unitOfMeasure.Id))
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
            return View(unitOfMeasure);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unitOfMeasure = _unitOfMeasureServices.GetId(id);
            if (unitOfMeasure == null)
            {
                return NotFound();
            }

            return View(unitOfMeasure);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOfMeasureServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool UnitOfMeasureExists(int id)
        {
            return _context.UnitOfMeasures.Any(e => e.Id == id);
        }
    }
}
