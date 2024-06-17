using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required, MaxLength(50)]
    public string LastName { get; set; }
    
    [Required, MaxLength(50)]
    public string City { get; set; }
    
    [Required, MaxLength(50)]
    public string Country { get; set; }
    
    [Required, Phone]
    public string Phone { get; set; }
    
    public ICollection<Order> Orders { get; set; }
}