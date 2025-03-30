using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailManagement.Models;

public class EmailBrowserGroup
{
    [Required]
    public string EmailId { get; set; } = string.Empty;
    
    [Required]
    public int BrowserGroupId { get; set; }
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    [ForeignKey("EmailId")]
    public virtual Email Email { get; set; } = null!;
    
    [ForeignKey("BrowserGroupId")]
    public virtual BrowserGroup BrowserGroup { get; set; } = null!;
}