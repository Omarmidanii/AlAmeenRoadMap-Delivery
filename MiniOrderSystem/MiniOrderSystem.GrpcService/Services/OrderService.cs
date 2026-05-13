using Grpc.Core;

namespace MiniOrderSystem.GrpcService.Services;

using MiniOrderSystem.Data;
using Microsoft.EntityFrameworkCore;
public class OrderServiceImpl : OrderService.OrderServiceBase
{
    private readonly AppDbContext _db;

    public OrderServiceImpl(AppDbContext db)
    {
        _db = db;
    }

    public override async Task<OrderReply> CreateOrder(
        CreateOrderRequest request,
        ServerCallContext context)
    {
        var order = new Order
        {
            CustomerName = request.CustomerName,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            Status = (int)OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return new OrderReply
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            Status = (OrderStatus)order.Status
        };
    }

    public override async Task<OrderReply> GetOrder(
    GetOrderRequest request,
    ServerCallContext context)
    {
        var order = await _db.Orders.FindAsync(request.Id);

        if (order == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
        }

        return new OrderReply
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            Status = (OrderStatus)order.Status
        };
    }

    public override async Task<ListOrdersReply> ListOrders(
       ListOrdersRequest request,
       ServerCallContext context)
    {
        var orders = await _db.Orders.ToListAsync();

        var reply = new ListOrdersReply();

        reply.Orders.AddRange(orders.Select(order => new OrderReply
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            Status = (OrderStatus)order.Status
        }));

        return reply;
    }

    public override async Task<OrderReply> CancelOrder(
        CancelOrderRequest request,
        ServerCallContext context)
    {
        var order = await _db.Orders.FindAsync(request.Id);

        if (order == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
        }

        order.Status = (int)OrderStatus.Cancelled;

        await _db.SaveChangesAsync();

        return new OrderReply
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            Status = (OrderStatus)order.Status
        };
    }
}

