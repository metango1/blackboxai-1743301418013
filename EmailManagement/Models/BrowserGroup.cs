using System.ComponentModel.DataAnnotations;

namespace EmailManagement.Models;

public class BrowserGroup
{
    [Key]
    public int BrowserGroupId { get; set; }
    
    [Required]
    [Display(Name = "Browser Group Name")]
    public string BrowserGroupName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public virtual ICollection<EmailBrowserGroup> EmailBrowserGroups { get; set; } = new List<EmailBrowserGroup>();
}