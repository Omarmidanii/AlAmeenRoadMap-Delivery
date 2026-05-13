# Stage 1: Build and compile the C# code
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Copy the project file and restore dependencies
COPY ["GrpcServer/GrpcServer.csproj", "GrpcServer/"]
RUN dotnet restore "GrpcServer/GrpcServer.csproj"
# Copy the rest of the code and build
COPY . .
WORKDIR "/src/GrpcServer"
RUN dotnet publish "GrpcServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GrpcServer.dll"] 