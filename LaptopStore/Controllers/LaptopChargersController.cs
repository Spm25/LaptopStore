using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data;
using LaptopStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace LaptopStore.Controllers
{
    [Authorize]
    public class LaptopChargersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LaptopChargersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LaptopChargers
        public async Task<IActionResult> Index()
        {
            return View(await _context.LaptopChargers.ToListAsync());
        }

        // GET: LaptopChargers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopCharger = await _context.LaptopChargers
                .FirstOrDefaultAsync(m => m.ChargerID == id);
            if (laptopCharger == null)
            {
                return NotFound();
            }

            return View(laptopCharger);
        }

        // GET: LaptopChargers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LaptopChargers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChargerID,Wattage,Connector,Quantity,Quality")] LaptopCharger laptopCharger)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptopCharger);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laptopCharger);
        }

        // GET: LaptopChargers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopCharger = await _context.LaptopChargers.FindAsync(id);
            if (laptopCharger == null)
            {
                return NotFound();
            }
            return View(laptopCharger);
        }

        // POST: LaptopChargers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChargerID,Wattage,Connector,Quantity,Quality")] LaptopCharger laptopCharger)
        {
            if (id != laptopCharger.ChargerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptopCharger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopChargerExists(laptopCharger.ChargerID))
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
            return View(laptopCharger);
        }

        // GET: LaptopChargers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopCharger = await _context.LaptopChargers
                .FirstOrDefaultAsync(m => m.ChargerID == id);
            if (laptopCharger == null)
            {
                return NotFound();
            }

            return View(laptopCharger);
        }

        // POST: LaptopChargers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptopCharger = await _context.LaptopChargers.FindAsync(id);
            if (laptopCharger != null)
            {
                _context.LaptopChargers.Remove(laptopCharger);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopChargerExists(int id)
        {
            return _context.LaptopChargers.Any(e => e.ChargerID == id);
        }
    }
}
