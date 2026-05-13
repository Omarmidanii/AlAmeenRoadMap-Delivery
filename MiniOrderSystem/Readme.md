# gRPC Training Project

In this porject, I created an example for a gRPC flow and learned how gRPC works, and here is a summary of what I've done and how we can run it.

## What I have done?
- A simulation for an order process, Adding and order, fetching a specific order, canceling an order and seeing all the orders.
- Done using gRPC, with SQL Server and EF core.

## The Structure of the project 
- **GrpcService** : here is the server, where I implemented the gRPC methods and added the .proto file for the order. Also, in the Services folder, I added the order class where the logic of each method is there. I am running this project this way:
```bash
dotnet run --project MiniOrderSystem.GrpcService --urls "http://localhost:5001"
```
- **DataBase** : for the database we need first to install the EF tool then run the migrations:
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```
 I am using SQL Server Managment, just take the connection string from the appsettings.json and paste it in the app and run connect, it has for now Orders table, where the table has: Id, CustomerName, ProductName, Quantity, Status, CreatedAt.
- **Docker** : We should run the container to make the sql server work and verify the project is running so we need to run:
```bash
docker compose up
```
- **Client** : here is the client that is calling the Service, it can add orders and fetch them and so on, here is how to run it:
```bash
dotnet run --project MiniOrderSystem.Client
```


