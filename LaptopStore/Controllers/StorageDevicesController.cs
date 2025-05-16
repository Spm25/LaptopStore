﻿using System;
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
    public class StorageDevicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StorageDevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StorageDevices
        public async Task<IActionResult> Index()
        {
            return View(await _context.StorageDevices.ToListAsync());
        }

        // GET: StorageDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageDevice = await _context.StorageDevices
                .FirstOrDefaultAsync(m => m.StorageID == id);
            if (storageDevice == null)
            {
                return NotFound();
            }

            return View(storageDevice);
        }

        // GET: StorageDevices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StorageDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StorageID,Type,Capacity,Quantity,Quality")] StorageDevice storageDevice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storageDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storageDevice);
        }

        // GET: StorageDevices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageDevice = await _context.StorageDevices.FindAsync(id);
            if (storageDevice == null)
            {
                return NotFound();
            }
            return View(storageDevice);
        }

        // POST: StorageDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StorageID,Type,Capacity,Quantity,Quality")] StorageDevice storageDevice)
        {
            if (id != storageDevice.StorageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storageDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageDeviceExists(storageDevice.StorageID))
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
            return View(storageDevice);
        }

        // GET: StorageDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageDevice = await _context.StorageDevices
                .FirstOrDefaultAsync(m => m.StorageID == id);
            if (storageDevice == null)
            {
                return NotFound();
            }

            return View(storageDevice);
        }

        // POST: StorageDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storageDevice = await _context.StorageDevices.FindAsync(id);
            if (storageDevice != null)
            {
                _context.StorageDevices.Remove(storageDevice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageDeviceExists(int id)
        {
            return _context.StorageDevices.Any(e => e.StorageID == id);
        }
    }
}
