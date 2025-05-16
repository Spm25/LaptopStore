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
    public class LaptopBatteriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LaptopBatteriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LaptopBatteries
        public async Task<IActionResult> Index()
        {
            return View(await _context.LaptopBatteries.ToListAsync());
        }

        // GET: LaptopBatteries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopBattery = await _context.LaptopBatteries
                .FirstOrDefaultAsync(m => m.BatteryID == id);
            if (laptopBattery == null)
            {
                return NotFound();
            }

            return View(laptopBattery);
        }

        // GET: LaptopBatteries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LaptopBatteries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatteryID,LaptopModel,Capacity,UsedStatus,Type,Quantity,Quality")] LaptopBattery laptopBattery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptopBattery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laptopBattery);
        }

        // GET: LaptopBatteries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopBattery = await _context.LaptopBatteries.FindAsync(id);
            if (laptopBattery == null)
            {
                return NotFound();
            }
            return View(laptopBattery);
        }

        // POST: LaptopBatteries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatteryID,LaptopModel,Capacity,UsedStatus,Type,Quantity,Quality")] LaptopBattery laptopBattery)
        {
            if (id != laptopBattery.BatteryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptopBattery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopBatteryExists(laptopBattery.BatteryID))
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
            return View(laptopBattery);
        }

        // GET: LaptopBatteries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopBattery = await _context.LaptopBatteries
                .FirstOrDefaultAsync(m => m.BatteryID == id);
            if (laptopBattery == null)
            {
                return NotFound();
            }

            return View(laptopBattery);
        }

        // POST: LaptopBatteries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptopBattery = await _context.LaptopBatteries.FindAsync(id);
            if (laptopBattery != null)
            {
                _context.LaptopBatteries.Remove(laptopBattery);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopBatteryExists(int id)
        {
            return _context.LaptopBatteries.Any(e => e.BatteryID == id);
        }
    }
}
