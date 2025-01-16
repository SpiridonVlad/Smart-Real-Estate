FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore "WebAPI/Real Estate Management System.csproj"
RUN dotnet publish "WebAPI/Real Estate Management System.csproj" -c Release -o /app --no-restore

# Use the .NET Runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_ENVIRONMENT=Development 

EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Real Estate Management System.dll"]
