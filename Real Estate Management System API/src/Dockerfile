# Use the .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy everything and restore dependencies specifically for WebAPI
COPY . .
RUN dotnet restore "WebAPI/Real Estate Management System.csproj"

# Publish the application
RUN dotnet publish "WebAPI/Real Estate Management System.csproj" -c Release -o /app

# Use the .NET Runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Set environment variable
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port 80
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Real Estate Management System.dll"]
