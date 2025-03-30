using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailManagement.Data;
using EmailManagement.Models;

namespace EmailManagement.Controllers;

public class BrowserGroupsController : Controller
{
    private readonly EmailManagementDbContext _context;
    private readonly ILogger<BrowserGroupsController> _logger;

    public BrowserGroupsController(EmailManagementDbContext context, ILogger<BrowserGroupsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: BrowserGroups
    public async Task<IActionResult> Index()
    {
        return View(await _context.BrowserGroups.OrderBy(bg => bg.BrowserGroupName).ToListAsync());
    }

    // GET: BrowserGroups/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BrowserGroups/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BrowserGroupName")] BrowserGroup browserGroup)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(browserGroup);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new browser group: {BrowserGroupName}", browserGroup.BrowserGroupName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating browser group");
                ModelState.AddModelError("", "Unable to create browser group. Please try again.");
            }
        }
        return View(browserGroup);
    }

    // GET: BrowserGroups/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var browserGroup = await _context.BrowserGroups.FindAsync(id);
        if (browserGroup == null)
        {
            return NotFound();
        }
        return View(browserGroup);
    }

    // POST: BrowserGroups/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BrowserGroupId,BrowserGroupName")] BrowserGroup browserGroup)
    {
        if (id != browserGroup.BrowserGroupId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(browserGroup);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated browser group: {BrowserGroupName}", browserGroup.BrowserGroupName);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BrowserGroupExists(browserGroup.BrowserGroupId))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating browser group");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating browser group");
                ModelState.AddModelError("", "Unable to update browser group. Please try again.");
            }
        }
        return View(browserGroup);
    }

    // GET: BrowserGroups/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var browserGroup = await _context.BrowserGroups
            .FirstOrDefaultAsync(m => m.BrowserGroupId == id);
        if (browserGroup == null)
        {
            return NotFound();
        }

        return View(browserGroup);
    }

    // POST: BrowserGroups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var browserGroup = await _context.BrowserGroups.FindAsync(id);
        if (browserGroup != null)
        {
            try
            {
                _context.BrowserGroups.Remove(browserGroup);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted browser group: {BrowserGroupName}", browserGroup.BrowserGroupName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting browser group");
                ModelState.AddModelError("", "Unable to delete browser group. Please try again.");
                return View(browserGroup);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    private bool BrowserGroupExists(int id)
    {
        return _context.BrowserGroups.Any(e => e.BrowserGroupId == id);
    }
}