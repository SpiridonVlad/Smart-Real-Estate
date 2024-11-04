# Step 1: Use .NET SDK 8.0 to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy each .csproj file and restore dependencies
COPY ["WebAPI/Real Estate Management System.csproj", "WebAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Run restore on the WebAPI project, which should include all dependencies
RUN dotnet restore "WebAPI/Real Estate Management System.csproj"

# Copy all source code into the container
COPY . .

# Change to the WebAPI directory to build the project
WORKDIR "/src/WebAPI"
RUN dotnet publish "Real Estate Management System.csproj" -c Release -o /app/publish

# Step 2: Use .NET 8.0 runtime to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port your app listens on
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "Real Estate Management System.dll"]
