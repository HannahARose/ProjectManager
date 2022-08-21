using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    public class ManagerEventTypeController : Controller
    {
        private const string folderPath = "~/Views/Manager/EventType/";

        private readonly ApplicationDbContext _context;

        public ManagerEventTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manager/EventType
        public async Task<IActionResult> Index()
        {
            TempData["isManager"] = true;

            return _context.EventTypes != null ? 
                          View(folderPath + "Index.cshtml", await _context.EventTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EventTypes'  is null.");
        }

        // GET: Manager/EventType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            TempData["isManager"] = true;

            if (id == null || _context.EventTypes == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes
                .FirstOrDefaultAsync(m => m.EventTypeId == id);
            if (eventType == null)
            {
                return NotFound();
            }

            return View(folderPath + "Details.cshtml", eventType);
        }

        // GET: Manager/EventType/Create
        public IActionResult Create()
        {
            TempData["isManager"] = true;

            return View(folderPath + "Create.cshtml");
        }

        // POST: Manager/EventType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventTypeId,Name,Description,ValueMultiplier")] EventType eventType)
        {
            TempData["isManager"] = true;

            if (ModelState.IsValid)
            {
                _context.Add(eventType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(folderPath + "Create.cshtml", eventType);
        }

        // GET: Manager/EventType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData["isManager"] = true;

            if (id == null || _context.EventTypes == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes.FindAsync(id);
            if (eventType == null)
            {
                return NotFound();
            }
            return View(folderPath + "Edit.cshtml", eventType);
        }

        // POST: Manager/EventType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventTypeId,Name,Description,ValueMultiplier")] EventType eventType)
        {
            TempData["isManager"] = true;

            if (id != eventType.EventTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventTypeExists(eventType.EventTypeId))
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
            return View(folderPath + "Edit.cshtml", eventType);
        }

        // GET: Manager/EventType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["isManager"] = true;

            if (id == null || _context.EventTypes == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes
                .FirstOrDefaultAsync(m => m.EventTypeId == id);
            if (eventType == null)
            {
                return NotFound();
            }

            return View(folderPath + "Delete.cshtml", eventType);
        }

        // POST: Manager/EventType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData["isManager"] = true;

            if (_context.EventTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EventTypes'  is null.");
            }
            var eventType = await _context.EventTypes.FindAsync(id);
            if (eventType != null)
            {
                _context.EventTypes.Remove(eventType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventTypeExists(int id)
        {
          return (_context.EventTypes?.Any(e => e.EventTypeId == id)).GetValueOrDefault();
        }
    }
}
