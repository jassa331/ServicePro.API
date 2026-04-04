# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the solution file first (for caching restores)
COPY ServicePro.API.sln ./

# Copy all project files
COPY ServicePro.API/ ServicePro.API/
COPY ServicePro.Core/ ServicePro.Core/
COPY ServicePro.Infrastructure/ ServicePro.Infrastructure/
COPY ServicePro.Services/ ServicePro.Services/

# Restore dependencies for the solution
RUN dotnet restore ServicePro.API.sln

# Copy all source code
COPY . .

# Build and publish the API project
RUN dotnet publish ServicePro.API/ServicePro.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish ./

# Configure ASP.NET Core to listen on dynamic port
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Run the API
ENTRYPOINT ["dotnet", "ServicePro.API.dll"]
