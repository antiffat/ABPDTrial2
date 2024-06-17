using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string ProductName { get; set; }
    
    public int SupplierId { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    [MaxLength(50)]
    public string Package { get; set; }
    
    public bool IsDiscounted { get; set; }
    
    public Supplier Supplier { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}