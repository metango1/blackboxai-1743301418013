using System.ComponentModel.DataAnnotations;

namespace EmailManagement.Models;

public class Tag
{
    [Key]
    public int TagId { get; set; }
    
    [Required]
    [Display(Name = "Tag Name")]
    public string TagName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public virtual ICollection<EmailTag> EmailTags { get; set; } = new List<EmailTag>();
}