using System.ComponentModel.DataAnnotations;

namespace EmailManagement.Models;

public class Email
{
    [Key]
    public string EmailId { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Tab Group")]
    public string TabGroup { get; set; } = string.Empty;
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<EmailTag> EmailTags { get; set; } = new List<EmailTag>();
    public virtual ICollection<EmailBrowserGroup> EmailBrowserGroups { get; set; } = new List<EmailBrowserGroup>();
    public virtual ICollection<EmailUseCase> EmailUseCases { get; set; } = new List<EmailUseCase>();
}