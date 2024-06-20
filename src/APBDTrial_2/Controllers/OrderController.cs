using APBDTrial_2.DTOs;
using APBDTrial_2.Helpers;
using APBDTrial_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBDTrial_2.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer) // Include() because customer entity is directly related to order entity
            .Include(o => o.OrderItems) // Include() because OrderItems entity is directly related to order entity
            .ThenInclude(oi => oi.Product) // ThenInclude() because Product is related to another included entity
            .ThenInclude(p => p.Supplier)
            .Select(order => new OrderDto
            {
                Items = order.OrderItems.Select(item => new OrderItemDetailDto
                {
                    ProductName = item.Product.ProductName,
                    SupplierCompanyName = item.Product.Supplier.CompanyName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList(),
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                FirstName = order.Customer.FirstName,
                LastName = order.Customer.LastName
            })
            .ToListAsync(cancellationToken);
        
        return Ok(orders);
    }
    
    // GET: api/orders/id
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p.Supplier)
            .Where(o => o.Id == id)
            .Select(order => new OrderDto
            {
                Items = order.OrderItems.Select(item => new OrderItemDetailDto
                {
                    ProductName = item.Product.ProductName,
                    SupplierCompanyName = item.Product.Supplier.CompanyName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList(),
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                FirstName = order.Customer.FirstName,
                LastName = order.Customer.LastName
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }
    
    // Does not work. Reason: OrderDate again is problematic and program assumes I insert null value to it.
    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.FirstName == createOrderDto.FirstName 
                && c.LastName == createOrderDto.LastName, cancellationToken);

        if (customer == null)
        {
            return BadRequest($"Customer '{createOrderDto.FirstName} {createOrderDto.LastName}' not found.");
        }

        var order = new Order
        {
            Customer = customer,
            OrderDate = DateTime.UtcNow,
            TotalAmount = 0m,
            OrderItems = new List<OrderItem>()
        };

        foreach (var itemDto in createOrderDto.Items)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductName == itemDto.ProductName
                                          && p.Supplier.CompanyName == itemDto.CompanyName, cancellationToken);

            if (product == null)
            {
                return BadRequest($"Product '{itemDto.ProductName}' from supplier '{itemDto.CompanyName}' not found.");
            }

            var orderItem = new OrderItem
            {
                Product = product,
                UnitPrice = product.UnitPrice,
                Quantity = itemDto.Quantity
            };

            order.OrderItems.Add(orderItem);
            order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);

        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}