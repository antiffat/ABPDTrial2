using System.ComponentModel.DataAnnotations;

namespace APBDTrial_2.Models;

public class Supplier
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string CompanyName { get; set; }
    
    [MaxLength(50)]
    public string ContactName { get; set; }
    
    [MaxLength(50)]
    public string City { get; set; }
    
    [MaxLength(50)]
    public string Country { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    
    [Phone]
    public string Fax { get; set; }
    
    [ConcurrencyCheck]
    public byte[] RowVersion { get; set; }
}