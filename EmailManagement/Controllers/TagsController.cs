using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailManagement.Data;
using EmailManagement.Models;

namespace EmailManagement.Controllers;

public class TagsController : Controller
{
    private readonly EmailManagementDbContext _context;
    private readonly ILogger<TagsController> _logger;

    public TagsController(EmailManagementDbContext context, ILogger<TagsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Tags
    public async Task<IActionResult> Index()
    {
        return View(await _context.Tags.OrderBy(t => t.TagName).ToListAsync());
    }

    // GET: Tags/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tags/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TagName")] Tag tag)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new tag: {TagName}", tag.TagName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tag");
                ModelState.AddModelError("", "Unable to create tag. Please try again.");
            }
        }
        return View(tag);
    }

    // GET: Tags/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tag = await _context.Tags.FindAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        return View(tag);
    }

    // POST: Tags/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TagId,TagName")] Tag tag)
    {
        if (id != tag.TagId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tag);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated tag: {TagName}", tag.TagName);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TagExists(tag.TagId))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating tag");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tag");
                ModelState.AddModelError("", "Unable to update tag. Please try again.");
            }
        }
        return View(tag);
    }

    // GET: Tags/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tag = await _context.Tags
            .FirstOrDefaultAsync(m => m.TagId == id);
        if (tag == null)
        {
            return NotFound();
        }

        return View(tag);
    }

    // POST: Tags/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag != null)
        {
            try
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted tag: {TagName}", tag.TagName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tag");
                ModelState.AddModelError("", "Unable to delete tag. Please try again.");
                return View(tag);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    private bool TagExists(int id)
    {
        return _context.Tags.Any(e => e.TagId == id);
    }
}