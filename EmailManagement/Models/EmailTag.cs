using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailManagement.Models;

public class EmailTag
{
    [Required]
    public string EmailId { get; set; } = string.Empty;
    
    [Required]
    public int TagId { get; set; }
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    [ForeignKey("EmailId")]
    public virtual Email Email { get; set; } = null!;
    
    [ForeignKey("TagId")]
    public virtual Tag Tag { get; set; } = null!;
}