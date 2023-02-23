using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize(Roles = "Admin, Nurse, Doctor, Pharmacist")]
    public class MedicinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicine.ToListAsync());
        }

        // GET: Medicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }

            var medicine = await _context.Medicine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicine == null)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }

            return View(medicine);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Cost,Description,Quantity,Restock")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }

            var medicine = await _context.Medicine.FindAsync(id);
            if (medicine == null)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Cost,Description,Quantity,Restock")] Medicine medicine)
        {
            if (id != medicine.ID)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.ID))
                    {
                        ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }

            var medicine = await _context.Medicine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicine == null)
            {
                ViewBag.ErrorMessage = $"Medicine with Id = {id} cannot be found";
                return View("NotFound");
            }

            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicine.FindAsync(id);
            _context.Medicine.Remove(medicine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicine.Any(e => e.ID == id);
        }
    }
}
