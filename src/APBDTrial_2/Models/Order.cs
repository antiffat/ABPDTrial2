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
    public byte[] RowVersion { get; set; } // I added concurrency check only in Order class as we will do operations on
                                           // Order mainly. Should I add that to all Models also?
    
    public Customer Customer { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}