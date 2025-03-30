using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailManagement.Data;
using EmailManagement.Models;

namespace EmailManagement.Controllers;

public class UseCasesController : Controller
{
    private readonly EmailManagementDbContext _context;
    private readonly ILogger<UseCasesController> _logger;

    public UseCasesController(EmailManagementDbContext context, ILogger<UseCasesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: UseCases
    public async Task<IActionResult> Index()
    {
        return View(await _context.UseCases.OrderBy(uc => uc.UseCaseName).ToListAsync());
    }

    // GET: UseCases/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: UseCases/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UseCaseName")] UseCase useCase)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(useCase);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new use case: {UseCaseName}", useCase.UseCaseName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating use case");
                ModelState.AddModelError("", "Unable to create use case. Please try again.");
            }
        }
        return View(useCase);
    }

    // GET: UseCases/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var useCase = await _context.UseCases.FindAsync(id);
        if (useCase == null)
        {
            return NotFound();
        }
        return View(useCase);
    }

    // POST: UseCases/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UseCaseId,UseCaseName")] UseCase useCase)
    {
        if (id != useCase.UseCaseId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(useCase);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated use case: {UseCaseName}", useCase.UseCaseName);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UseCaseExists(useCase.UseCaseId))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating use case");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating use case");
                ModelState.AddModelError("", "Unable to update use case. Please try again.");
            }
        }
        return View(useCase);
    }

    // GET: UseCases/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var useCase = await _context.UseCases
            .FirstOrDefaultAsync(m => m.UseCaseId == id);
        if (useCase == null)
        {
            return NotFound();
        }

        return View(useCase);
    }

    // POST: UseCases/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var useCase = await _context.UseCases.FindAsync(id);
        if (useCase != null)
        {
            try
            {
                _context.UseCases.Remove(useCase);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted use case: {UseCaseName}", useCase.UseCaseName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting use case");
                ModelState.AddModelError("", "Unable to delete use case. Please try again.");
                return View(useCase);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    private bool UseCaseExists(int id)
    {
        return _context.UseCases.Any(e => e.UseCaseId == id);
    }
}