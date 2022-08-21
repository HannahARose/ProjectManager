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
    public class ManagerTaskController : Controller
    {
        private const string folderPath = "~/Views/Manager/Task/";

        private readonly ApplicationDbContext _context;

        public ManagerTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manager/Task
        public async Task<IActionResult> Index(int? ProjectId)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            IEnumerable<Project> projects = _context.Projects.ToList().Prepend(new Project
            {
                ProjectId = -1,
                Name = "--All--"
            });

            ViewData["ProjectId"] = new SelectList(projects, "ProjectId", "Name", ProjectId ?? -1);


            var applicationDbContext = _context.NTasks.Where(x => (ProjectId.HasValue && ProjectId != -1) ? x.ProjectId == ProjectId : true ).Include(n => n.Project);
            return View(folderPath + "Index.cshtml", await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public ActionResult TaskTable(string ProjectId)
        {
            List<NTask> model;
            if (Int32.TryParse(ProjectId, out int pid) && pid != -1)
            {
                ViewData["DisplayProjects"] = false;
                model = _context.NTasks.Where(x => x.ProjectId == pid).ToList();
            }
            else
            {
                ViewData["DisplayProjects"] = true;
                model = _context.NTasks.ToList();
                model.ForEach(x => x.Project = _context.Projects.Where(y => y.ProjectId == x.ProjectId).Single());
            }
            return PartialView(folderPath + "TaskTable.cshtml", model);
        }

        // GET: Manager/Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            if (id == null || _context.NTasks == null)
            {
                return NotFound();
            }

            var nTask = await _context.NTasks
                .Include(n => n.Project)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (nTask == null)
            {
                return NotFound();
            }

            return View(folderPath + "Details.cshtml", nTask);
        }

        // GET: Manager/Task/Create
        public IActionResult Create()
        {
            TempData["isManager"] = User.IsInRole("Manager");

            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name");
            return View(folderPath + "Create.cshtml");
        }

        // POST: Manager/Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Name,Description,ProjectId")] NTask nTask)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            if (ModelState.IsValid)
            {
                _context.Add(nTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", nTask.ProjectId);
            return View(folderPath + "Create.cshtml", nTask);
        }

        // GET: Manager/Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            if (id == null || _context.NTasks == null)
            {
                return NotFound();
            }

            var nTask = await _context.NTasks.FindAsync(id);
            if (nTask == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", nTask.ProjectId);
            return View(folderPath + "Edit.cshtml", nTask);
        }

        // POST: Manager/Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Name,Description,ProjectId")] NTask nTask)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            if (id != nTask.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NTaskExists(nTask.TaskId))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", nTask.ProjectId);
            return View(folderPath + "Edit.cshtml", nTask);
        }

        // GET: Manager/Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            if (id == null || _context.NTasks == null)
            {
                return NotFound();
            }

            var nTask = await _context.NTasks
                .Include(n => n.Project)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (nTask == null)
            {
                return NotFound();
            }

            return View(folderPath + "Delete.cshtml", nTask);
        }

        // POST: Manager/Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData["isManager"] = User.IsInRole("Manager");

            if (_context.NTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NTasks'  is null.");
            }
            var nTask = await _context.NTasks.FindAsync(id);
            if (nTask != null)
            {
                _context.NTasks.Remove(nTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NTaskExists(int id)
        {
          return (_context.NTasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
        }
    }
}
