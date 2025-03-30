using Microsoft.EntityFrameworkCore;
using EmailManagement.Models;

namespace EmailManagement.Data;

public class EmailManagementDbContext : DbContext
{
    public EmailManagementDbContext(DbContextOptions<EmailManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Email> Emails { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<BrowserGroup> BrowserGroups { get; set; } = null!;
    public DbSet<UseCase> UseCases { get; set; } = null!;
    public DbSet<EmailTag> EmailTags { get; set; } = null!;
    public DbSet<EmailBrowserGroup> EmailBrowserGroups { get; set; } = null!;
    public DbSet<EmailUseCase> EmailUseCases { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Email
        modelBuilder.Entity<Email>()
            .HasKey(e => e.EmailId);

        // Configure EmailTag junction
        modelBuilder.Entity<EmailTag>()
            .HasKey(et => new { et.EmailId, et.TagId });

        modelBuilder.Entity<EmailTag>()
            .HasOne(et => et.Email)
            .WithMany(e => e.EmailTags)
            .HasForeignKey(et => et.EmailId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmailTag>()
            .HasOne(et => et.Tag)
            .WithMany(t => t.EmailTags)
            .HasForeignKey(et => et.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure EmailBrowserGroup junction
        modelBuilder.Entity<EmailBrowserGroup>()
            .HasKey(ebg => new { ebg.EmailId, ebg.BrowserGroupId });

        modelBuilder.Entity<EmailBrowserGroup>()
            .HasOne(ebg => ebg.Email)
            .WithMany(e => e.EmailBrowserGroups)
            .HasForeignKey(ebg => ebg.EmailId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmailBrowserGroup>()
            .HasOne(ebg => ebg.BrowserGroup)
            .WithMany(bg => bg.EmailBrowserGroups)
            .HasForeignKey(ebg => ebg.BrowserGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure EmailUseCase junction
        modelBuilder.Entity<EmailUseCase>()
            .HasKey(euc => new { euc.EmailId, euc.UseCaseId });

        modelBuilder.Entity<EmailUseCase>()
            .HasOne(euc => euc.Email)
            .WithMany(e => e.EmailUseCases)
            .HasForeignKey(euc => euc.EmailId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmailUseCase>()
            .HasOne(euc => euc.UseCase)
            .WithMany(uc => uc.EmailUseCases)
            .HasForeignKey(euc => euc.UseCaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure indexes
        modelBuilder.Entity<Email>()
            .HasIndex(e => e.TabGroup)
            .HasDatabaseName("idx_emails_tab_group");

        modelBuilder.Entity<EmailTag>()
            .HasIndex(et => et.EmailId)
            .HasDatabaseName("idx_email_tags_email_id");

        modelBuilder.Entity<EmailTag>()
            .HasIndex(et => et.TagId)
            .HasDatabaseName("idx_email_tags_tag_id");

        modelBuilder.Entity<EmailBrowserGroup>()
            .HasIndex(ebg => ebg.EmailId)
            .HasDatabaseName("idx_email_browser_groups_email_id");

        modelBuilder.Entity<EmailBrowserGroup>()
            .HasIndex(ebg => ebg.BrowserGroupId)
            .HasDatabaseName("idx_email_browser_groups_browser_group_id");

        modelBuilder.Entity<EmailUseCase>()
            .HasIndex(euc => euc.EmailId)
            .HasDatabaseName("idx_email_usecases_email_id");

        modelBuilder.Entity<EmailUseCase>()
            .HasIndex(euc => euc.UseCaseId)
            .HasDatabaseName("idx_email_usecases_usecase_id");
    }
}