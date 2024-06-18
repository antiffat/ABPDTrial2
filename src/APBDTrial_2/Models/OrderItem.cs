using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int ProductId { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    [ConcurrencyCheck]
    public byte[] RowVersion { get; set; }
    
    public Order Order { get; set; }
    
    public Product Product { get; set; }
}