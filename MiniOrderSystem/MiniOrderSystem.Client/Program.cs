using Grpc.Core;
using Grpc.Net.Client;
using MiniOrderSystem.GrpcService;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");

var client = new OrderService.OrderServiceClient(channel);


var list = await client.ListOrdersAsync(new ListOrdersRequest());

Console.WriteLine($"Total orders: {list.Orders.Count}");

foreach (var order in list.Orders)
{
    Console.WriteLine(
        $"#{order.Id} | {order.CustomerName} | {order.ProductName} | {order.Quantity} | {order.Status}"
    );
}
// Console.WriteLine("Created order:");
// Console.WriteLine($"Order #{created.Id}, Status: {created.Status}");

// var fetched = await client.GetOrderAsync(new GetOrderRequest
// {
//     Id = created.Id
// });

// Console.WriteLine();
// Console.WriteLine("Fetched order:");
// Console.WriteLine($"Order #{fetched.Id}");
// Console.WriteLine($"Customer: {fetched.CustomerName}");
// Console.WriteLine($"Product: {fetched.ProductName}");
// Console.WriteLine($"Quantity: {fetched.Quantity}");
// Console.WriteLine($"Status: {fetched.Status}");

// try
// {
//     var missing = await client.GetOrderAsync(new GetOrderRequest
//     {
//         Id = 999
//     });

//     Console.WriteLine($"Found order #{missing.Id}");
// }
// catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
// {
//     Console.WriteLine("Order not found");
// }