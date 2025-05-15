using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data;
using LaptopStore.Models;

namespace LaptopStore.Controllers
{
    public class LaptopScreensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LaptopScreensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LaptopScreens
        public async Task<IActionResult> Index()
        {
            return View(await _context.LaptopScreens.ToListAsync());
        }

        // GET: LaptopScreens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopScreen = await _context.LaptopScreens
                .FirstOrDefaultAsync(m => m.ScreenID == id);
            if (laptopScreen == null)
            {
                return NotFound();
            }

            return View(laptopScreen);
        }

        // GET: LaptopScreens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LaptopScreens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScreenID,Resolution,ScreenType,UsedStatus,Repaired,Quantity,Quality")] LaptopScreen laptopScreen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptopScreen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laptopScreen);
        }

        // GET: LaptopScreens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopScreen = await _context.LaptopScreens.FindAsync(id);
            if (laptopScreen == null)
            {
                return NotFound();
            }
            return View(laptopScreen);
        }

        // POST: LaptopScreens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScreenID,Resolution,ScreenType,UsedStatus,Repaired,Quantity,Quality")] LaptopScreen laptopScreen)
        {
            if (id != laptopScreen.ScreenID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptopScreen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopScreenExists(laptopScreen.ScreenID))
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
            return View(laptopScreen);
        }

        // GET: LaptopScreens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopScreen = await _context.LaptopScreens
                .FirstOrDefaultAsync(m => m.ScreenID == id);
            if (laptopScreen == null)
            {
                return NotFound();
            }

            return View(laptopScreen);
        }

        // POST: LaptopScreens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptopScreen = await _context.LaptopScreens.FindAsync(id);
            if (laptopScreen != null)
            {
                _context.LaptopScreens.Remove(laptopScreen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopScreenExists(int id)
        {
            return _context.LaptopScreens.Any(e => e.ScreenID == id);
        }
    }
}
