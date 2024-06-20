namespace APBDTrial_2.DTOs;

public class OrderDto
{
    public List<OrderItemDetailDto> Items { get; set; }
    public DateTime OrderDate { get; set; }
    public int TotalAmount { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}