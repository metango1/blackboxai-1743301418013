using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailManagement.Data;
using EmailManagement.Models;

namespace EmailManagement.Controllers;

public class EmailsController : Controller
{
    private readonly EmailManagementDbContext _context;
    private readonly ILogger<EmailsController> _logger;

    public EmailsController(EmailManagementDbContext context, ILogger<EmailsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Emails
    public async Task<IActionResult> Index(string searchEmail, string searchTabGroup, int? searchTagId, 
        int? searchBrowserGroupId, int? searchUseCaseId)
    {
        try
        {
            var query = _context.Emails
                .Include(e => e.EmailTags)
                    .ThenInclude(et => et.Tag)
                .Include(e => e.EmailBrowserGroups)
                    .ThenInclude(ebg => ebg.BrowserGroup)
                .Include(e => e.EmailUseCases)
                    .ThenInclude(euc => euc.UseCase)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchEmail))
            {
                query = query.Where(e => e.EmailId.Contains(searchEmail));
            }

            if (!string.IsNullOrEmpty(searchTabGroup))
            {
                query = query.Where(e => e.TabGroup.Contains(searchTabGroup));
            }

            if (searchTagId.HasValue)
            {
                query = query.Where(e => e.EmailTags.Any(et => et.TagId == searchTagId));
            }

            if (searchBrowserGroupId.HasValue)
            {
                query = query.Where(e => e.EmailBrowserGroups.Any(ebg => ebg.BrowserGroupId == searchBrowserGroupId));
            }

            if (searchUseCaseId.HasValue)
            {
                query = query.Where(e => e.EmailUseCases.Any(euc => euc.UseCaseId == searchUseCaseId));
            }

            // Get all lookup data for dropdowns
            ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
            ViewBag.BrowserGroups = await _context.BrowserGroups.OrderBy(bg => bg.BrowserGroupName).ToListAsync();
            ViewBag.UseCases = await _context.UseCases.OrderBy(uc => uc.UseCaseName).ToListAsync();

            var emails = await query.OrderByDescending(e => e.UpdatedAt).ToListAsync();
            return View(emails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving emails");
            return RedirectToAction(nameof(Error));
        }
    }

    // GET: Emails/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var email = await _context.Emails
            .Include(e => e.EmailTags)
                .ThenInclude(et => et.Tag)
            .Include(e => e.EmailBrowserGroups)
                .ThenInclude(ebg => ebg.BrowserGroup)
            .Include(e => e.EmailUseCases)
                .ThenInclude(euc => euc.UseCase)
            .FirstOrDefaultAsync(e => e.EmailId == id);

        if (email == null)
        {
            return NotFound();
        }

        return View(email);
    }

    // GET: Emails/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.BrowserGroups = await _context.BrowserGroups.OrderBy(bg => bg.BrowserGroupName).ToListAsync();
        ViewBag.UseCases = await _context.UseCases.OrderBy(uc => uc.UseCaseName).ToListAsync();
        return View();
    }

    // POST: Emails/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Email email, int[] selectedTags, int[] selectedBrowserGroups, int[] selectedUseCases)
    {
        if (ModelState.IsValid)
        {
            try
            {
                email.CreatedAt = DateTime.UtcNow;
                email.UpdatedAt = DateTime.UtcNow;

                // Add selected tags
                foreach (var tagId in selectedTags)
                {
                    email.EmailTags.Add(new EmailTag { TagId = tagId });
                }

                // Add selected browser groups
                foreach (var browserGroupId in selectedBrowserGroups)
                {
                    email.EmailBrowserGroups.Add(new EmailBrowserGroup { BrowserGroupId = browserGroupId });
                }

                // Add selected use cases
                foreach (var useCaseId in selectedUseCases)
                {
                    email.EmailUseCases.Add(new EmailUseCase { UseCaseId = useCaseId });
                }

                _context.Add(email);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating email");
                ModelState.AddModelError("", "Unable to create email. Please try again.");
            }
        }

        // If we got this far, something failed, redisplay form
        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.BrowserGroups = await _context.BrowserGroups.OrderBy(bg => bg.BrowserGroupName).ToListAsync();
        ViewBag.UseCases = await _context.UseCases.OrderBy(uc => uc.UseCaseName).ToListAsync();
        return View(email);
    }

    // GET: Emails/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var email = await _context.Emails
            .Include(e => e.EmailTags)
            .Include(e => e.EmailBrowserGroups)
            .Include(e => e.EmailUseCases)
            .FirstOrDefaultAsync(e => e.EmailId == id);

        if (email == null)
        {
            return NotFound();
        }

        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.BrowserGroups = await _context.BrowserGroups.OrderBy(bg => bg.BrowserGroupName).ToListAsync();
        ViewBag.UseCases = await _context.UseCases.OrderBy(uc => uc.UseCaseName).ToListAsync();

        ViewBag.SelectedTags = email.EmailTags.Select(et => et.TagId).ToArray();
        ViewBag.SelectedBrowserGroups = email.EmailBrowserGroups.Select(ebg => ebg.BrowserGroupId).ToArray();
        ViewBag.SelectedUseCases = email.EmailUseCases.Select(euc => euc.UseCaseId).ToArray();

        return View(email);
    }

    // POST: Emails/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Email email, int[] selectedTags, int[] selectedBrowserGroups, int[] selectedUseCases)
    {
        if (id != email.EmailId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingEmail = await _context.Emails
                    .Include(e => e.EmailTags)
                    .Include(e => e.EmailBrowserGroups)
                    .Include(e => e.EmailUseCases)
                    .FirstOrDefaultAsync(e => e.EmailId == id);

                if (existingEmail == null)
                {
                    return NotFound();
                }

                // Update basic properties
                existingEmail.FirstName = email.FirstName;
                existingEmail.LastName = email.LastName;
                existingEmail.TabGroup = email.TabGroup;
                existingEmail.UpdatedAt = DateTime.UtcNow;

                // Update tags
                existingEmail.EmailTags.Clear();
                foreach (var tagId in selectedTags)
                {
                    existingEmail.EmailTags.Add(new EmailTag { TagId = tagId });
                }

                // Update browser groups
                existingEmail.EmailBrowserGroups.Clear();
                foreach (var browserGroupId in selectedBrowserGroups)
                {
                    existingEmail.EmailBrowserGroups.Add(new EmailBrowserGroup { BrowserGroupId = browserGroupId });
                }

                // Update use cases
                existingEmail.EmailUseCases.Clear();
                foreach (var useCaseId in selectedUseCases)
                {
                    existingEmail.EmailUseCases.Add(new EmailUseCase { UseCaseId = useCaseId });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while updating email");
                if (!EmailExists(email.EmailId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating email");
                ModelState.AddModelError("", "Unable to update email. Please try again.");
            }
        }

        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.BrowserGroups = await _context.BrowserGroups.OrderBy(bg => bg.BrowserGroupName).ToListAsync();
        ViewBag.UseCases = await _context.UseCases.OrderBy(uc => uc.UseCaseName).ToListAsync();
        return View(email);
    }

    // GET: Emails/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var email = await _context.Emails
            .Include(e => e.EmailTags)
                .ThenInclude(et => et.Tag)
            .Include(e => e.EmailBrowserGroups)
                .ThenInclude(ebg => ebg.BrowserGroup)
            .Include(e => e.EmailUseCases)
                .ThenInclude(euc => euc.UseCase)
            .FirstOrDefaultAsync(e => e.EmailId == id);

        if (email == null)
        {
            return NotFound();
        }

        return View(email);
    }

    // POST: Emails/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            var email = await _context.Emails.FindAsync(id);
            if (email != null)
            {
                _context.Emails.Remove(email);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting email");
            return RedirectToAction(nameof(Error));
        }
    }

    private bool EmailExists(string id)
    {
        return _context.Emails.Any(e => e.EmailId == id);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}