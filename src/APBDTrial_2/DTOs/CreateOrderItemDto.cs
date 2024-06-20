using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.DTOs;

public class CreateOrderItemDto
{
    [Required, MaxLength(100)]
    public string ProductName { get; set; }
    
    [Required, MaxLength(100)]
    public string CompanyName { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}