using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.DTOs;

public class CreateOrderDto
{
    [Required]
    public List<CreateOrderItemDto> Items { get; set; }
    
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required, MaxLength(50)]
    public string LastName { get; set; }
}