using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailManagement.Models;

public class EmailUseCase
{
    [Required]
    public string EmailId { get; set; } = string.Empty;
    
    [Required]
    public int UseCaseId { get; set; }
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    [ForeignKey("EmailId")]
    public virtual Email Email { get; set; } = null!;
    
    [ForeignKey("UseCaseId")]
    public virtual UseCase UseCase { get; set; } = null!;
}