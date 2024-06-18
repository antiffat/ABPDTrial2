using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Timestamp]
    public DateTime OrderDate { get; set; }
    
    public int CustomerId { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }
    
    [ConcurrencyCheck]
    public byte[] RowVersion { get; set; }
    
    public Customer Customer { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}