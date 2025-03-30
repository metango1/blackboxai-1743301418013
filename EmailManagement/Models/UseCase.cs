using System.ComponentModel.DataAnnotations;

namespace EmailManagement.Models;

public class UseCase
{
    [Key]
    public int UseCaseId { get; set; }
    
    [Required]
    [Display(Name = "Use Case Name")]
    public string UseCaseName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public virtual ICollection<EmailUseCase> EmailUseCases { get; set; } = new List<EmailUseCase>();
}