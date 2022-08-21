using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    [Authorize(Roles="Manager")]
    public class ManagerProjectController : Controller
    {
        private const string folderPath = "~/Views/Manager/Project/";
        private readonly ApplicationDbContext _context;

        public ManagerProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manager/Project
        public async Task<IActionResult> Index()
        {
            TempData["isManager"] = true;

            return _context.Projects != null ? 
                          View(folderPath+"Index.cshtml", await _context.Projects.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
        }

        // GET: Manager/Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            TempData["isManager"] = true;

            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(folderPath+"Details.cshtml", project);
        }

        // GET: Manager/Project/Create
        public IActionResult Create()
        {
            TempData["isManager"] = true;

            return View(folderPath+"Create.cshtml");
        }

        // POST: Manager/Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Description")] Project project)
        {
            TempData["isManager"] = true;

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(folderPath+"Create.cshtml", project);
        }

        // GET: Manager/Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData["isManager"] = true;

            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(folderPath+"Edit.cshtml", project);
        }

        // POST: Manager/Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description")] Project project)
        {
            TempData["isManager"] = true;

            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(folderPath + "Edit.cshtml", project);
        }

        // GET: Manager/Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["isManager"] = true;

            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(folderPath + "Delete.cshtml", project);
        }

        // POST: Manager/Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData["isManager"] = true;

            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
          return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
