namespace APBDTrial_2.DTOs;

public class OrderItemDetailDto
{
    public string ProductName { get; set; }
    public string SupplierCompanyName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}